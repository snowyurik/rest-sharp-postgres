using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Model;
using Microsoft.EntityFrameworkCore;
// using Npgsql;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using( var db = new Context() ) {
                db.Database.EnsureCreated();
                db.Database.Migrate(); 
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
