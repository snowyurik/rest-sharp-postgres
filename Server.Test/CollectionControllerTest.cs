using System;
using Xunit;
using Server.Model;
using Server.Misc;
using Server.Controllers;
using Server.DataLib.Model;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Server.Test {

    [Collection("Sequential")]
    public class CollectionControllerTest : Common.Test {

        [Fact]
        public void testSelf() {
            Assert.True(true, "check suite");
        }

        [Fact]
        public void voidTestController() {
            // CollectionController controller = new CollectionController();
            // var result = controller.create( "book", new Book {
            //     Title = "title 1",
            //     Description = "book description",
            // } );
            // Log( ((Book)((JsonResult)result).Value).Id );
            // // Book addedItem = JsonConvert.DeserializeObject<Book>( result.ToString() );
            // // Log( addedItem.Id );
            // result = controller.delete( "book", ((Book)((JsonResult)result).Value).Id );
            // Log( result );
        }

        [Fact]
        public void testGetCollectionByName() {
            // CollectionController controller = new CollectionController();
            // var books = controller.getCollectionByName("book");
            // Log( books.GetType() );
            // Assert.True( books == controller.getContext().Books );
            // var shelfs = controller.getCollectionByName("shelf");
            // Log( shelfs.GetType() );
            // Assert.True( shelfs == controller.getContext().Shelfs );
        }

    }

}