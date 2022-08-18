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
using Microsoft.EntityFrameworkCore;
using JsonElement = System.Text.Json.JsonElement;
// using System.Text.Json;

namespace Server.Controllers {

    public class EItemNotFound : Exception {
        public EItemNotFound(string msg) : base(msg) {}
    }

    [ApiController]
    [Route("/")]
    public class CollectionController : ControllerBase {

        private Context _context = new Context();
        private Dictionary<string, IRepository> _repoMap = new Dictionary<string, IRepository> { // TODO replace with Attributes and reflection
            { "book", new Repository<Book>() }
        };
        public Context getContext() {
            return _context;
        }

        public IRepository getRepository(string collection) {
            return _repoMap[collection];
        }

        private IActionResult _callWrapper(Func<IActionResult> callback) {
            try {
                return callback();
            } catch( Exception e) {
                return Problem(
                    statusCode: 400,
                    title: e.GetType().Name,
                    detail: e.Message
                );
            }
        }

        /**
        Post is RESTfult Create
        */
        [HttpPost("{collection}")]
        public IActionResult create( [FromRoute] string collection, [FromBody] JsonElement item ) {
            return _callWrapper( ()=>{ return getRepository(collection).create( item ); });
        }

        [HttpGet("{collection}/{id}")]
        public IActionResult get( string collection, int id ) {
            return _callWrapper( ()=>{ return getRepository(collection).get( id ); });
        }

        [HttpPut("{collection}/{id}")]
        public IActionResult update([FromRoute] string collection, [FromBody] JsonElement item) {
            return _callWrapper( ()=>{ return getRepository(collection).update(item); });
        }

        [HttpDelete("{collection}/{id}")]
        public IActionResult delete([FromRoute] string collection, [FromRoute] int id ) {
            return _callWrapper( ()=>{ return getRepository(collection).delete( id ); });
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
                using (var db = _context ) {
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
          
    }
}