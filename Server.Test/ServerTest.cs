using System;
using Xunit;
using Server.Model;

namespace Server.Test
{
    public class ServerTest
    {
        [Fact]
        public void testSelf() {
            Assert.True( true, "check if we can run suite ");
        }

        [Fact]
        public void TestBooksCRUDForDb() {
            // using (var db = new Context()) {
            //         // Create and save a new Blog
            //         Console.Write("Enter a name for a new Blog: ");
            //         var name = Console.ReadLine();

            //         var blog = new Blog { Name = name };
            //         db.Blogs.Add(blog);
            //         db.SaveChanges();

            //         // Display all Blogs from the database
            //         var query = from b in db.Blogs
            //                     orderby b.Name
            //                     select b;

            //         Console.WriteLine("All blogs in the database:");
            //         foreach (var item in query)
            //         {
            //             Console.WriteLine(item.Name);
            //         }

            //         Console.WriteLine("Press any key to exit...");
            //         Console.ReadKey();
            //     }
            // }
        }
    }
}
