using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using IdentityServer4.EntityFramework.Storage;

namespace Icon.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<SeedData>>();
            // TODO Shall we really migrate here?
            services.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            services.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            /* using (var persistedGrantDbContext = new PersistedGrantDbContext( */
            /*     services.GetRequiredService<DbContextOptions<PersistedGrantDbContext>>())) */
            /* { */
            /*   persistedGrantDbContext.Database.Migrate(); */
            /* } */
            /* using (var configurationDbContext = new ConfigurationDbContext( */
            /*     services.GetRequiredService<DbContextOptions<ConfigurationDbContext>>())) */
            /* { */
            var context = services.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                logger.LogDebug("Clients being populated");
                foreach (var client in Config.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                logger.LogDebug("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                logger.LogDebug("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                logger.LogDebug("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                logger.LogDebug("ApiResources being populated");
                foreach (var resource in Config.GetApis().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                logger.LogDebug("ApiResources already populated");
            }
        }
    }
}