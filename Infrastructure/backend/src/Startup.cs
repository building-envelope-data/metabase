using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
/* using AutoMapper; */

namespace Infrastructure
{
    public class Startup
    {
        protected readonly IWebHostEnvironment _environment;
        protected readonly IConfiguration _configuration;
        protected readonly AppSettings _appSettings;

        public Startup(
            IWebHostEnvironment environment,
            string[] commandLineArguments
            )
        {
            _environment = environment;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "XBASE_") // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#environment-variables
                .AddCommandLine(commandLineArguments)
              .Build();
            _appSettings = _configuration.Get<AppSettings>();
        }

        protected string GetAssemblyName()
        {
            return GetType().GetTypeInfo().Assembly.GetName().Name.NotNull();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            /* services.AddAutoMapper(GetType()); */
            services.AddSingleton<AppSettings>(_appSettings);
        }
    }
}