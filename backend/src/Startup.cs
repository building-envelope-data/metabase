using NSwag.Generation.Processors.Security;
using IdentityServer4.AccessTokenValidation;
using Icon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Microsoft.OpenApi;
using NSwag;
using IdentityServer4.AspNetIdentity;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Command = Icon.Infrastructure.Command;
using Query = Icon.Infrastructure.Query;
using Event = Icon.Infrastructure.Event;
using Domain = Icon.Domain;
using Component = Icon.Domain.Component;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Configuration = Icon.Configuration;

// TODO ? Certificate authentication: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-3.0
// TODO Use NodaTime (see commented code below; confer https://www.npgsql.org/efcore/mapping/nodatime.html).
//      There are problems with the generated migrations of the identity server.
//      Maybe https://github.com/etiennemtl/identity-npsql-nodatime helps.
//      The error was `System.InvalidCastException: Can't write CLR type System.DateTime with handler type TimestampHandler`
//      See also https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/568
//      https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/648
// TODO API versioning: https://github.com/RicoSuter/NSwag/issues/2118

// OpenIddict is another implementation of OpenId Connect

// IdentityServer4 Demo: https://demo.identityserver.io/
//											 https://github.com/IdentityServer/IdentityServer4.Demo/blob/master/src/IdentityServer4Demo/Config.cs

// TODO IdentityServer4 Admin UI. Available options are:
//			https://github.com/skoruba/IdentityServer4.Admin
//			https://github.com/brunohbrito/JPProject.IdentityServer4.AdminUI
//			https://github.com/zarxor/IdentityServer4.OpenAdmin

// TODO For client libraries use https://identitymodel.readthedocs.io/en/latest/

// TODO Swagger UI for IdentityServer4 endpoints, follow `https://github.com/IdentityServer/IdentityServer4/issues/2286`

namespace Icon
{
    public class Startup
    {
        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;
        private AppSettings _appSettings;

        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
              .Build();
            _appSettings = _configuration.Get<AppSettings>();
        }

        private string GetMigrationsAssembly()
        {
            return typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>(_appSettings);
            ConfigureDatabaseServices(services);
            Configuration.Session.ConfigureServices(services);
            Configuration.RequestResponse.ConfigureServices(services);
            Configuration.Api.ConfigureServices(services);
            Configuration.Auth.ConfigureServices(services, _environment, _configuration, _appSettings, GetMigrationsAssembly());
            Configuration.GraphQl.ConfigureServices(services);
            Configuration.EventStore.ConfigureServices(services, _environment, _appSettings.Database);
            Configuration.QueryCommandAndEventBusses.ConfigureServices(services);
        }

        private void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(_ =>
                {
                    _.UseNpgsql(_appSettings.Database.ConnectionString /* , o => o.UseNodaTime() */)
                      .EnableSensitiveDataLogging(_appSettings.Logging.EnableSensitiveDataLogging);
                }
              );
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.0
        public void Configure(IApplicationBuilder app)
        {
            Configuration.RequestResponse.ConfigureRouting(app, _environment);
            Configuration.Api.Configure(app);
            Configuration.Auth.Configure(app);
            Configuration.Session.Configure(app);
            Configuration.GraphQl.Configure(app);
            Configuration.RequestResponse.ConfigureEndpoints(app);

            // TODO Shall we do migrations here or in Program.cs?
            /* app.ApplicationServices.GetService<ClientsDbContext>().Database.Migrate(); */
        }
    }
}