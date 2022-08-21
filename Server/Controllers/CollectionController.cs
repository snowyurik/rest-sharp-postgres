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
using System.Linq.Dynamic.Core;

// using System.Text.Json;

namespace Server.Controllers {

    public class EItemNotFound : Exception {
        public EItemNotFound(string msg) : base(msg) {}
    }

    /**
    this controller provide universal rest api for all data models defined in Server.DataLib.Model 
    which are based on Server.DataLib.Model.BaseItem class
    to add new table - just add new data class there, add it to Server.Model.Context and run
        dotnet ef migrations add %migration name%
    */
    [ApiController]
    [Route("/")]
    public class CollectionController : ControllerBase {

        private Context _context = new Context();
        private Dictionary<string, IRepository> _repoMap;

        /**
        adding all classes deritaed from BaseItem which we have in Server.DataLib assembly
        */
        public CollectionController() {
            _repoMap = new Dictionary<string, IRepository>();
            var types = Assembly.GetAssembly( typeof(BaseItem) ).GetTypes()
                .Where( t => t.BaseType == typeof(BaseItem));
            foreach( Type itemType in types ) {
                Type repoType = typeof( Repository<> );
                Type genericType = repoType.MakeGenericType(itemType);
                _repoMap.Add( itemType.Name.ToLower(), (IRepository)Activator.CreateInstance( genericType ) );
            }
        }

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

        [HttpGet("{collection}")]
        public IActionResult getList( string collection ) {
            return _callWrapper( ()=>{ return getRepository(collection).getList( this.Request.Query ); });
        }

          
    }
}