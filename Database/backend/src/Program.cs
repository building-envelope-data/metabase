using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Directory = System.IO.Directory;

namespace Database
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
                  webBuilder
                  .UseKestrel()
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseStartup<Startup>()
                  );
        }
    }
}