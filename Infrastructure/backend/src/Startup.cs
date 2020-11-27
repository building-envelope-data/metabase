using System;
using System.Reflection;
using Infrastructure.Extensions;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            RegisterJsonSchemaFiles();
        }

        private void RegisterJsonSchemaFiles()
        {
            // We force initialization of `JsonSchemaRegistry` on start-up as
            // otherwise errors with the JSON Schemas are only encountered on
            // the registry's first usage initiated by a GraphQL query and that
            // query also takes rather long (because the JSON Schemas are
            // loaded, validated, and registered).
            var count = ValueObjects.JsonSchema.JsonSchemaRegistry.Count;
            if (count is 0)
            {
                throw new Exception("There are no JSON Schemas in the registry");
            }
        }

        protected string GetMigrationsAssembly()
        {
            return GetType().GetTypeInfo().Assembly.GetName().Name.NotNull();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>(_appSettings);
        }
    }
}