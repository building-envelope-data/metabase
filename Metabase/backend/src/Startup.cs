using System;
using System.Reflection;
using Metabase.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
//                       https://github.com/IdentityServer/IdentityServer4.Demo/blob/master/src/IdentityServer4Demo/Config.cs

// TODO IdentityServer4 Admin UI. Available options are:
//      https://github.com/skoruba/IdentityServer4.Admin
//      https://github.com/brunohbrito/JPProject.IdentityServer4.AdminUI
//      https://github.com/zarxor/IdentityServer4.OpenAdmin

// TODO For client libraries use https://identitymodel.readthedocs.io/en/latest/

// TODO Swagger UI for IdentityServer4 endpoints, follow `https://github.com/IdentityServer/IdentityServer4/issues/2286`

namespace Metabase
{
    public class Startup
                : Infrastructure.Startup
    {
        public Startup(
            IWebHostEnvironment environment,
            string[] commandLineArguments
            )
          : base(
              environment,
              commandLineArguments
              )
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            Infrastructure.Configuration.Session.ConfigureServices(services);
            Infrastructure.Configuration.RequestResponse.ConfigureServices(services);
            Configuration.Auth.ConfigureServices(services, _environment, _configuration, _appSettings, GetMigrationsAssembly());
            Configuration.GraphQl.ConfigureServices(services);
            Configuration.Database.ConfigureServices(services, _appSettings.Database);
        }

        public void Configure(IApplicationBuilder app)
        {
            Infrastructure.Configuration.RequestResponse.ConfigureRouting(app, _environment);
            Configuration.Auth.Configure(app);
            Infrastructure.Configuration.Session.Configure(app);

            // TODO Shall we do migrations here or in Program.cs?
            /* app.ApplicationServices.GetService<ClientsDbContext>().Database.Migrate(); */
        }
    }
}