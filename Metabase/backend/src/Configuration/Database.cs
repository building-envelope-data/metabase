using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Infrastructure.Data;

namespace Metabase.Configuration
{
    public sealed class Database
    {
        public static void ConfigureServices(
            IServiceCollection services,
            Infrastructure.AppSettings.DatabaseSettings databaseSettings
            )
        {
          services.AddPooledDbContextFactory<Data.ApplicationDbContext>(options =>
              options
              .UseNpgsql(databaseSettings.ConnectionString)
              .UseSchemaName(databaseSettings.SchemaName)
              .UseOpenIddict()
              /* .UseNodaTime() */ // https://www.npgsql.org/efcore/mapping/nodatime.html
              );
          // Database contexts are used by `Identity`.
          services.AddDbContext<Data.ApplicationDbContext>(
              (services, options) => services.GetRequiredService<IDbContextFactory<Data.ApplicationDbContext>>().CreateDbContext(),
              ServiceLifetime.Transient
              );
        }
    }
}
