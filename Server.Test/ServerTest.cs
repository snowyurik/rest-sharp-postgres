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
    [Collection("Sequential")]
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
                var countBefore = db.Books.Count();
                Book book = new Book { 
                    Title = "test book title 1",
                    Description = ""
                    };
                db.Books.Add( book );
                db.SaveChanges();
                var countAdded = db.Books.Count();
                Assert.Equal( countBefore+1, countAdded );

                var query = from b in db.Books
                            .OrderByDescending( b=>b.Id )
                            .Take(1)
                            select b;
                Book bookFromDb = query.FirstOrDefault();
                Assert.Equal( book, bookFromDb );
                db.Remove( book );
                db.SaveChanges();
                var countAfter = db.Books.Count();
                Assert.Equal( countBefore, countAfter );
            }
        }

    }
}
