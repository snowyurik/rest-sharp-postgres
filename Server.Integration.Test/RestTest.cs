using System;
using Xunit;
using Server.ClientLib;
using Server.DataLib.Model;
using Server.DataLib.Filter;
using System.Linq;
using System.Collections.Generic;

namespace Server.Integration.Test {

    public class RestTest : Common.Test, IDisposable {
        private Service service = new Service();

        [Fact]
        public void testCrudREST() {
            service.start();
            IClient client = new Client{
                Url = service.getBaseUrl()
            };
            Book book = new Book {
                Title = "title 1",
                Description = "book description"
            };
            book.Id = client.add<Book>( book );
            Book bookFromServer = client.getList<Book>( limit:1, offset:0 ).FirstOrDefault();
            Assert.Equal( book.ToJsonString(), bookFromServer.ToJsonString() );
            bookFromServer.Title = "modified title";
            client.update( bookFromServer );
            Book modifiedBook = client.get<Book>( id: bookFromServer.Id );
            Assert.Equal( bookFromServer.ToJsonString(), modifiedBook.ToJsonString() );
            client.remove<Book>( id: bookFromServer.Id );
            Assert.Throws<EClientItemNotFound>(()=> client.get<Book>( bookFromServer.Id ) );
        }

        [Fact]
        public void testIsRead() {
            service.start();
            IClient client = new Client{
                Url = service.getBaseUrl()
            };
            Book book = new Book {
                Title = "title 1",
                Description = "book description",
                Read = false,
            };
            List<Book> results = client.getList<Book>( filter: new BookFilter{ Read=true }  );
            int countBefore = results.Count;
            book.Id = client.add<Book>( book );
            results = client.getList<Book>( filter: new BookFilter{ Read=true }  );
            int countAfterAdd = results.Count;
            Assert.Equal( countBefore, countAfterAdd ); // new book is not read
            book.Read = true;
            client.update<Book>( book );
            results = client.getList<Book>( filter: new BookFilter{ Read=true }  );
            int countAfterUpdate = results.Count;
            Assert.Equal( countBefore+1, countAfterUpdate );
            client.remove<Book>( book.Id );            
        }

        [Fact]
        public void testSearchFullTitlePartialTitle() {
            service.start();
            IClient client = new Client{
                Url = service.getBaseUrl()
            };
            Book book = new Book {
                Title = "title full",
                Description = "book description",
            };
            List<Book> results = client.getList<Book>( filter: new BookFilter{ Title = "title" }  );
            int countBefore = results.Count;
            results = client.getList<Book>( filter: new BookFilter{ Title = "like:\"title\"" }  );
            int countPartialBefore = results.Count;
            results = client.getList<Book>( filter: new BookFilter{ Title = "title full" }  );
            int countFullBefore = results.Count;

            book.Id = client.add<Book>( book );
            
            results = client.getList<Book>( filter: new BookFilter{ Title = "title" }  );
            Assert.Equal( countBefore, results.Count ); // "title" != "title full"
            results = client.getList<Book>( filter: new BookFilter{ Title = "title full" }  );
            Assert.Equal( countFullBefore+1, results.Count );
            results = client.getList<Book>( filter: new BookFilter{ Title = "like:\"title\"" }  );
            Assert.Equal( countPartialBefore+1, results.Count );
            client.remove<Book>( book.Id );
        }

        public void Dispose() {
            service.stop();
        }
    }
}
