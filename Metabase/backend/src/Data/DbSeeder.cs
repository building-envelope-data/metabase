using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace Metabase.Data
{
    public sealed class DbSeeder
    {
        public static async Task DoAsync(
            IServiceProvider services,
            bool testEnvironment
            )
        {
            var logger = services.GetRequiredService<ILogger<DbSeeder>>();
            logger.LogDebug("Seeding the database");
            await RegisterApplicationsAsync(services, logger, testEnvironment).ConfigureAwait(false);
            await RegisterScopesAsync(services, logger).ConfigureAwait(false);
        }

        private static async Task RegisterApplicationsAsync(
            IServiceProvider services,
            ILogger<DbSeeder> logger,
            bool testEnvironment
            )
        {
            var manager = services.GetRequiredService<IOpenIddictApplicationManager>();
            if (await manager.FindByClientIdAsync("metabase").ConfigureAwait(false) is null)
            {
                logger.LogDebug("Creating application client 'metabase'");
                await manager.CreateAsync(
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = "metabase",
                        ClientSecret = "secret", // TODO Do not hardcode secret here. It is also needed in tests, see `IntegrationTests#RequestAuthToken`
                        ConsentType = testEnvironment ? OpenIddictConstants.ConsentTypes.Systematic : OpenIddictConstants.ConsentTypes.Explicit,
                        DisplayName = "Metabase client application",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "Metabase-Klient-Anwendung"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri(testEnvironment ? "urn:test" : "https://localhost:4041/signout-callback-oidc") // TODO Use correct URI
                    },
                        RedirectUris =
                        {
                            new Uri(testEnvironment ? "urn:test" : "https://localhost:4041/signin-oidc") // TODO Use correct URI
                    },
                        Permissions = {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            // OpenIddictConstants.Permissions.Endpoints.Device,
                            // OpenIddictConstants.Permissions.Endpoints.Introspection,
                            OpenIddictConstants.Permissions.Endpoints.Logout,
                            // OpenIddictConstants.Permissions.Endpoints.Revocation,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            testEnvironment ? OpenIddictConstants.Permissions.GrantTypes.Password : OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            // OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                            // OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            testEnvironment ? OpenIddictConstants.Permissions.ResponseTypes.Token : OpenIddictConstants.Permissions.ResponseTypes.Code,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles,
                            OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.Auth.ApiScope,
                        },
                        Requirements =
                        {
                            OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                        }
                    }
                ).ConfigureAwait(false);
            }
        }

        private static async Task RegisterScopesAsync(
            IServiceProvider services,
            ILogger<DbSeeder> logger
            )
        {
            var manager = services.GetRequiredService<IOpenIddictScopeManager>();
            if (await manager.FindByNameAsync(Configuration.Auth.ApiScope).ConfigureAwait(false) is null)
            {
                logger.LogDebug($"Creating scope '{Configuration.Auth.ApiScope}'");
                await manager.CreateAsync(
                    new OpenIddictScopeDescriptor
                    {
                        DisplayName = "API access",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "API Zugriff"
                        },
                        Name = Configuration.Auth.ApiScope,
                        Resources =
                        {
                            Configuration.Auth.ServerName
                        }
                    }
                ).ConfigureAwait(false);
            }
        }
    }
}