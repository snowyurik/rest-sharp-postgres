using System.Data.Entity;

namespace Server.Model {
    public class Context : DbContext {
        public DbSet<Book> Books { get; set; }
    }
}