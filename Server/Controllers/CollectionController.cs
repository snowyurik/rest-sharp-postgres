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
                    // Book book = JsonConvert.DeserializeObject<Book>( item.ToString() );
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
        [HttpGet("{collection}")]
        public IActionResult getList( string collection ) {
             using (var db = new Context() ) {

                var query = from b in db.Books
                            .OrderByDescending( b=>b.Id )
                            // .Take(IItem.DEFAULT_LIMIT)
                            select b;
                return new JsonResult( query.ToList<Book>() );
            }
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