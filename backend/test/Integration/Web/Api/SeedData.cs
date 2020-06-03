using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ApplicationDbContext = Icon.Data.ApplicationDbContext;
using Configuration = Icon.Configuration;
using ConfigurationDbContext = IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext;
using Models = Icon.Models;
using PersistedGrantDbContext = IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext;
using Seeds = Icon.Data.Seeds;

namespace Test.Integration.Web.Api
{
    public static class SeedData
    {
        public static async Task SeedUsers(UserManager<Models.UserX> userManager)
        {
            var identityResult = await userManager.CreateAsync(
                    new Models.UserX()
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