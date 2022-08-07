using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Model;
using Server.DataLib.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DataLib.Attributes;
using System.Reflection;

namespace Server.Controllers {

    public class EItemNotFound : Exception {
        public EItemNotFound(string msg) : base(msg) {}
    }

    [ApiController]
    [Route("/")]
    public class CollectionController : ControllerBase {

        /**
        Post is RESTfult Create
        */
        [HttpPost("{collection}")]
        public IActionResult create( [FromRoute] string collection, [FromBody] Book book ) {
            try {
                using (var db = new Context() ) {   
                    db.Books.Add( book );
                    db.SaveChanges();
                    return new JsonResult( book );
                }
            } catch( Exception e) {
                return Problem(
                    statusCode: 400,
                    title: e.GetType().Name,
                    detail: e.Message
                );
            }
        }


        /**
        Get list of items
        */
#nullable enable
        [HttpGet("{collection}")]
        public IActionResult getList( string collection, [FromQuery] bool? read = null, [FromQuery] string? title = null ) {
#nullable disable
            try {
                ( bool like, string partialTitle ) = isLike( title );
                if(like) {
                    partialTitle = partialTitle.Trim('"').Replace("\\\"","\"");
                }
                using (var db = new Context() ) {
                    var query = from b in db.Books
                                .OrderByDescending( b=>b.Id )
                                .Where( b=> ( // TODO whis can be done better, but time is up 
                                        ( read == null && title == null )
                                        || ( title == null && b.Read == read )
                                        || ( ( read == null || b.Read == read ) && ( 
                                                ( !like && b.Title == title ) 
                                                || ( like && b.Title.Contains(partialTitle)) 
                                            ))
                                    )) 
                                select b;
                    return new JsonResult( query.ToList<Book>() );
                }
            } catch( Exception e) {
                return Problem(
                    statusCode: 400,
                    title: e.GetType().Name,
                    detail: e.Message + " \n\n "+ e.StackTrace
                );
            }
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

        [HttpGet("{collection}/{id}")]
        public IActionResult get( string collection, int id ) {
            using( var db = new Context() ) {
                var query = from b in db.Books
                    .Where( b=>b.Id == id)
                    .Take(1)
                    select b;
                if( query.Count<Book>() != 1) {
                    return NotFound();
                }
                Book book = query.First<Book>();
                return new JsonResult( book );
            }
        }

        [HttpPut("{collection}/{id}")]
        public IActionResult update([FromRoute] string collection, [FromBody] Book book) {
            using (var db = new Context() ) {
                db.Update( book );
                db.SaveChanges();
                return new JsonResult( book );
            }
        }

        [HttpDelete("{collection}/{id}")]
        public IActionResult delete([FromRoute] string collection, [FromRoute] int id ) {
            using (var db = new Context() ) {
                var query = from b in db.Books
                    .Where( b=>b.Id == id)
                    .Take(1)
                    select b;
                Book book = query.FirstOrDefault<Book>();
                db.Remove( book );
                db.SaveChanges();
                return new JsonResult( null );
            }
        }

          
    }
}