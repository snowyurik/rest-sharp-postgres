using System;
using Xunit;
using Server.ClientLib;
using Server.DataLib.Model;
using System.Linq;

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

        public void Dispose() {
            service.stop();
        }
    }
}
