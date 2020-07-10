using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Metabase.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<SeedData>>();
            /* // TODO Shall we really migrate here? */
            /* services.GetRequiredService<ApplicationDbContext>().Database.Migrate(); */
            /* services.GetRequiredService<PersistedGrantDbContext>().Database.Migrate(); */
            /* /1* using (var persistedGrantDbContext = new PersistedGrantDbContext( *1/ */
            /* /1*     services.GetRequiredService<DbContextOptions<PersistedGrantDbContext>>())) *1/ */
            /* /1* { *1/ */
            /* /1*   persistedGrantDbContext.Database.Migrate(); *1/ */
            /* /1* } *1/ */
            /* /1* using (var configurationDbContext = new ConfigurationDbContext( *1/ */
            /* /1*     services.GetRequiredService<DbContextOptions<ConfigurationDbContext>>())) *1/ */
            /* /1* { *1/ */
            /* var context = services.GetRequiredService<ConfigurationDbContext>(); */
            /* context.Database.Migrate(); */
            /* if (!context.Clients.Any()) */
            /* { */
            /*     logger.LogDebug("Clients being populated"); */
            /*     foreach (var client in Seeds.Auth.GetClients().ToList()) */
            /*     { */
            /*         context.Clients.Add(client.ToEntity()); */
            /*     } */
            /*     context.SaveChanges(); */
            /* } */
            /* else */
            /* { */
            /*     logger.LogDebug("Clients already populated"); */
            /* } */

            /* if (!context.IdentityResources.Any()) */
            /* { */
            /*     logger.LogDebug("IdentityResources being populated"); */
            /*     foreach (var resource in Seeds.Auth.GetIdentityResources().ToList()) */
            /*     { */
            /*         context.IdentityResources.Add(resource.ToEntity()); */
            /*     } */
            /*     context.SaveChanges(); */
            /* } */
            /* else */
            /* { */
            /*     logger.LogDebug("IdentityResources already populated"); */
            /* } */

            /* if (!context.ApiResources.Any()) */
            /* { */
            /*     logger.LogDebug("ApiResources being populated"); */
            /*     foreach (var resource in Seeds.Auth.GetApis().ToList()) */
            /*     { */
            /*         context.ApiResources.Add(resource.ToEntity()); */
            /*     } */
            /*     context.SaveChanges(); */
            /* } */
            /* else */
            /* { */
            /*     logger.LogDebug("ApiResources already populated"); */
            /* } */
        }
    }
}