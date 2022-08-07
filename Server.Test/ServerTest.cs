using System;
using Xunit;
using Server.Model;
using Server.DataLib.Model;
using Server.Misc;
using System.Linq;
// using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Server.Test
{
    public class ServerTest : Common.Test
    {
        [Fact]
        public void testSelf() {
            Assert.True( true, "check if we can run suite ");
        }

        [Fact]
        public void TestBooksCRUDForDb() {
            using (var db = new Context() ) {
                db.Database.EnsureCreated();
                db.Database.Migrate(); 
                Log("Context created");
                var countBefore = db.Books.Count();
                Log( countBefore );
                Book book = new Book { Title = "test book title 1" };
                db.Books.Add( book );
                db.SaveChanges();
                Log("Book saved ");
                Log( book );
                var countAdded = db.Books.Count();
                Log( countAdded );
                Assert.Equal( countBefore+1, countAdded );

                var query = from b in db.Books
                            .OrderByDescending( b=>b.Id )
                            .Take(1)
                            select b;
                
                Log("db query created");
                foreach( var item in query ) {
                    Log( item );
                }
                Book bookFromDb = query.FirstOrDefault();
                Log( bookFromDb );
                Assert.Equal( book, bookFromDb );
                db.Remove( book );
                db.SaveChanges();
                var countAfter = db.Books.Count();
                Log( countAfter );
                Assert.Equal( countBefore, countAfter );
            }
        }

    }
}
