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
        public static void Main(string[] commandLineArguments)
        {
            CreateHostBuilder(commandLineArguments).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] commandLineArguments)
        {
            return Host.CreateDefaultBuilder(commandLineArguments)
              .ConfigureWebHostDefaults(webBuilder =>
                  webBuilder
                  .UseKestrel()
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseStartup<Startup>(webHostBuilderContext =>
                    new Startup(
                      webHostBuilderContext.HostingEnvironment,
                      commandLineArguments
                      )
                    )
                  );
        }
    }
}