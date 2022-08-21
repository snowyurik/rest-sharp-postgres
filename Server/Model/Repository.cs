using Microsoft.EntityFrameworkCore;
using Server.DataLib.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace Server.Model {

    public interface IRepository {
        public abstract IActionResult create(JsonElement json);
        public abstract IActionResult get(int id);
        public abstract IActionResult update(JsonElement json);
        public abstract IActionResult delete(int id);
        public abstract IActionResult getList(Microsoft.AspNetCore.Http.IQueryCollection query);
    }
    /**
    wrapper for DbSet<...>
    */
    public class Repository<T> : IRepository where T : BaseItem {
        private Context _context = new Context();
        public IActionResult create(JsonElement json) {
            T item = JsonSerializer.Deserialize<T>(json);
            _context.Set<T>().Add( item );
            _context.SaveChanges();
            return new JsonResult( item );
        }

        public IActionResult get(int id) {
            DbSet<T> items = _context.Set<T>();
            var query = from b in items
                .Where( b=>b.Id == id)
                .Take(1)
                select b;
            if( query.Count<T>() != 1) {
                return new NotFoundResult();
            }
            T item = query.First<T>();
            return new JsonResult( item );
        }

        public IActionResult update(JsonElement json) {
            T item = JsonSerializer.Deserialize<T>(json);
            _context.Set<T>().Update( item );
            _context.SaveChanges();
            return new JsonResult( item );
        }

        public IActionResult delete(int id) {
            var query = from b in _context.Set<T>()
                .Where( b=>b.Id == id)
                .Take(1)
                select b;
            T item = query.FirstOrDefault<T>();
            _context.Remove( item );
            _context.SaveChanges();
            return new JsonResult( null );
        }

        public IActionResult getList(Microsoft.AspNetCore.Http.IQueryCollection query) {
            List<FormattableString> clauses = new List<FormattableString>();
            clauses.Add($" true ");
            PropertyInfo[] ps = typeof(T).GetProperties();
            foreach( PropertyInfo p in ps) {
                string key = p.Name.ToLower();
                string value = query[key];
                if( String.IsNullOrEmpty(value) ) {
                    continue;
                }

                if( p.PropertyType == typeof(bool) ) {
                    bool boolValue = Boolean.Parse( value );
                    clauses.Add($"{key} = {boolValue}");
                }
                if( p.PropertyType == typeof(string)) {
                    ( bool like, string partialValue ) = isLike( value );
                    if( like ) {
                        clauses.Add($" title.Contains({partialValue})");
                    }
                    if( !like){
                        clauses.Add($" title = \"{value}\" ");
                    }
                }
            }
            IOrderedQueryable<T> sql = _context.Set<T>()
                        .Where(String.Join(" AND ", clauses))
                        .OrderBy("id DESC");
            return new JsonResult( sql.ToList<T>() );
        }

#nullable enable
        private (bool, string) isLike( string? needle ) {
#nullable disable
            if( needle == null ) {
                return ( false, needle );
            }
            string[] exploded = needle.Split(":");
            return ( exploded.FirstOrDefault() == "like", String.Join(":", exploded.Skip(1).ToArray()) );
        }

    }
}