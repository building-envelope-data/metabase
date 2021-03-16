using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Metabase.Configuration
{
    public sealed class Database
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            Infrastructure.AppSettings.DatabaseSettings databaseSettings
            )
        {
            services.AddPooledDbContextFactory<Data.ApplicationDbContext>(options =>
                {
                    options
                    .UseNpgsql(databaseSettings.ConnectionString)
                    .UseSchemaName(databaseSettings.SchemaName)
                    .UseOpenIddict();
                    /* .UseNodaTime() */ // https://www.npgsql.org/efcore/mapping/nodatime.html
                    if (!environment.IsProduction())
                    {
                        options
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                    }
                }
                );
            // Database context as services are used by `Identity` and `OpenIddict`.
            services.AddDbContext<Data.ApplicationDbContext>(
                (services, options) =>
                {
                    if (!environment.IsProduction())
                    {
                        options
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                    }
                    services
                    .GetRequiredService<IDbContextFactory<Data.ApplicationDbContext>>()
                    .CreateDbContext();
                },
                ServiceLifetime.Transient
                );
        }
    }
}