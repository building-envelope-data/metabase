using System;
using ApplicationDbContext = Icon.Data.ApplicationDbContext;
using PersistedGrantDbContext = IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext;
using ConfigurationDbContext = IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext;
using Microsoft.AspNetCore.Identity;
using Seeds = Icon.Data.Seeds;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using IdentityServer4;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configuration = Icon.Configuration;
using Models = Icon.Models;

namespace Test.Integration.Web.Api
{
    public static class SeedData
    {
        public static async Task SeedUsers(UserManager<Models.User> userManager)
        {
            var identityResult = await userManager.CreateAsync(
                    new Models.User()
                    {
                        /* Id = 1, */
                        UserName = "simon@icon.com",
                        Email = "simon@icon.com",
                        EmailConfirmed = true,
                        /* Created = DateTime.UtcNow, */
                    },
                    "simonSIMON123@"
                    );
            // TODO fail if `!identityResult.Succeeded` and report `identityResult.Errors`, see https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.identityresult?view=aspnetcore-3.0
        }

        public static void SeedAuth(ConfigurationDbContext dbContext)
        {
            foreach (var client in Seeds.Auth.GetClients().ToList())
            {
                dbContext.Clients.Add(client.ToEntity());
            }
            dbContext.Clients.Add(
                    MakeResourceOwnerClient().ToEntity()
                    );

            foreach (var resource in Seeds.Auth.GetIdentityResources().ToList())
            {
                dbContext.IdentityResources.Add(resource.ToEntity());
            }

            foreach (var resource in Seeds.Auth.GetApis().ToList())
            {
                dbContext.ApiResources.Add(resource.ToEntity());
            }

            dbContext.SaveChanges();
        }

        private static Client MakeResourceOwnerClient()
        {
            return new Client
            {
                ClientId = "test",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                ClientSecrets =
                    {
                    new Secret("secret".Sha256())
                    },
                AllowedScopes =
                    {
                    Configuration.Auth.ApiName
                    }
            };
        }
    }
}