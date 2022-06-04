using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace PruebaWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
           // CreateHostBuilder(args).Build().Run();
            CreateWebHostBuilder(args).Build().Run();

        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
         .ConfigureLogging((hostingContext, logging) =>
         {
             logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
             logging.AddConsole();
             logging.AddDebug();
             logging.AddEventSourceLogger();
          //   logging.AddNLog();



         }).UseStartup<Startup>();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
