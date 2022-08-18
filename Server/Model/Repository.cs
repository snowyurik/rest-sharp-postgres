using Microsoft.EntityFrameworkCore;
using Server.DataLib.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;


namespace Server.Model {

    public interface IRepository {
        public abstract IActionResult create(JsonElement json);
        public abstract IActionResult get(int id);
        public abstract IActionResult update(JsonElement json);
        public abstract IActionResult delete(int id);
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

    }
}