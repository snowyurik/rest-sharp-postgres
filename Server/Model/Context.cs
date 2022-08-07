using Microsoft.EntityFrameworkCore;
using Server.Misc;
using System;
using Server.DataLib.Model;

namespace Server.Model {

    public class Context : DbContext {

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            builder
                .UseNpgsql( Config.getStatic("dbconnection") )
                // .LogTo(Console.WriteLine)
                .UseSnakeCaseNamingConvention();
        }
    }
}