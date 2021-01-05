using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace Metabase.Data
{
    public sealed class DbSeeder
    {
        public static async Task Do(
            IServiceProvider services
            )
        {
            // var logger = services.GetRequiredService<ILogger<DbSeeder>>();
            await RegisterApplicationsAsync(services).ConfigureAwait(false);
            await RegisterScopesAsync(services).ConfigureAwait(false);
        }

        private static async Task RegisterApplicationsAsync(IServiceProvider services)
        {
            var manager = services.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("mvc").ConfigureAwait(false) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "mvc",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "MVC client application",
                    DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "MVC Client Anwendung"
                        },
                    PostLogoutRedirectUris =
                        {
                            new Uri("https://localhost:5000/signout-callback-oidc")
                        },
                    RedirectUris =
                        {
                            new Uri("https://localhost:5000/signin-oidc")
                        },
                    Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Logout,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            OpenIddictConstants.Permissions.ResponseTypes.Code,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles,
                            OpenIddictConstants.Permissions.Prefixes.Scope + "demo_api"
                        },
                    Requirements =
                        {
                            OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                        }
                }).ConfigureAwait(false);
            }

            // To test this sample with Postman, use the following settings:
            //
            // * Authorization URL: https://localhost:44395/connect/authorize
            // * Access token URL: https://localhost:44395/connect/token
            // * Client ID: postman
            // * Client secret: [blank] (not used with public clients)
            // * Scope: openid email profile roles
            // * Grant type: authorization code
            // * Request access token locally: yes
            if (await manager.FindByClientIdAsync("postman").ConfigureAwait(false) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ConsentType = OpenIddictConstants.ConsentTypes.Systematic,
                    DisplayName = "Postman",
                    RedirectUris =
                        {
                            new Uri("urn:postman")
                        },
                    Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Device,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                            OpenIddictConstants.Permissions.GrantTypes.Password,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            OpenIddictConstants.Permissions.ResponseTypes.Code,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles,
                        }
                }).ConfigureAwait(false);
            }
        }

        private static async Task RegisterScopesAsync(IServiceProvider services)
        {
            var manager = services.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("demo_api").ConfigureAwait(false) is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "Demo API access",
                    DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "Demo API Zugriff"
                        },
                    Name = "demo_api",
                    Resources =
                        {
                            "resource_server"
                        }
                }).ConfigureAwait(false);
            }
        }
    }
}
