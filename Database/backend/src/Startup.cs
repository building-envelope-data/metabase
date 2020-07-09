using System;
using System.Reflection;
using Infrastructure.Extensions;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public class Startup
            : Infrastructure.Startup
    {
        public Startup(IWebHostEnvironment environment)
                    : base(environment)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            Infrastructure.Configuration.Session.ConfigureServices(services);
            Infrastructure.Configuration.RequestResponse.ConfigureServices(services);
            Configuration.GraphQl.ConfigureServices(services);
            Configuration.EventStore.ConfigureServices(services, _environment, _appSettings.Database);
            Configuration.QueryCommandAndEventBusses.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1
        public void Configure(IApplicationBuilder app)
        {
            Infrastructure.Configuration.RequestResponse.ConfigureRouting(app, _environment);
            Infrastructure.Configuration.Session.Configure(app);
            Configuration.GraphQl.Configure(app, _environment);

            // TODO Shall we do migrations here or in Program.cs?
            /* app.ApplicationServices.GetService<ClientsDbContext>().Database.Migrate(); */
        }
    }
}