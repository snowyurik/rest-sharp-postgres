using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Server.Misc;
using System;
using Server.DataLib.Model;


using Npgsql;

namespace Server.Model {

    // class NpgSqlConfiguration : DbConfiguration
    // {
    //     public NpgSqlConfiguration()
    //     {
    //         var name = "Npgsql";

    //         SetProviderFactory(providerInvariantName: name,
    //                         providerFactory: NpgsqlFactory.Instance);

    //         SetProviderServices(providerInvariantName: name,
    //                             provider: NpgsqlServices.Instance);

    //         SetDefaultConnectionFactory(connectionFactory: new NpgsqlConnectionFactory());
    //     }
    // }

    public class Context : DbContext {

        public DbSet<Book> Books { get; set; }

        // public Context() : base("SchoolDB-EF6CodeFirst")
        // {
        //     Database.SetInitializer<Context>(new Server.Model.DBInitializer());
        // }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            Console.WriteLine("==="+Config.getStatic("dbconnection")+"===");
            builder
                .UseNpgsql( Config.getStatic("dbconnection") )
                // .LogTo(Console.WriteLine)
                .UseSnakeCaseNamingConvention();
        }
    }
}