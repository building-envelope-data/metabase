using System;
using System.Reflection;
using System.Text.Json;
using Metabase.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// TODO Certificate authentication: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth

// TODO For client libraries use https://identitymodel.readthedocs.io/en/latest/

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
            services.AddHealthChecks();
            Infrastructure.Configuration.Session.ConfigureServices(services);
            Infrastructure.Configuration.RequestResponse.ConfigureServices(services);
            Configuration.Auth.ConfigureServices(services, _environment, _configuration, _appSettings, GetMigrationsAssembly());
            Configuration.GraphQl.ConfigureServices(services);
            Configuration.Database.ConfigureServices(services, _appSettings.Database);
        }

        public void Configure(IApplicationBuilder app)
        {
            Infrastructure.Configuration.RequestResponse.ConfigureRouting(app, _environment);
            Infrastructure.Configuration.Session.Configure(app);
            Configuration.Auth.Configure(app);
            app.UseHealthChecks(
                "/health",
                new HealthCheckOptions {
                ResponseWriter = JsonResponseWriter
                }
                );
            /* app.UseWebSockets(); */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
            // TODO Shall we do migrations here or in Program.cs?
            /* app.ApplicationServices.GetService<ClientsDbContext>().Database.Migrate(); */
        }

        private async Task JsonResponseWriter(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                new {
                Status = report.Status.ToString()
                },
                new JsonSerializerOptions
                {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
                );
        }
    }
}