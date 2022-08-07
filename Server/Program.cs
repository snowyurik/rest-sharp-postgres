using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.Model;
using Microsoft.EntityFrameworkCore;

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
