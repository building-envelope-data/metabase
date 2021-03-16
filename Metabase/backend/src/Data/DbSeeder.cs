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
                        ClientSecret = "secret", // TODO Do not hardcode secret here. It is also needed in tests, see `IntegrationTests#RequestAuthToken`. See also frontend `.env.*` `AUTH_CLIENT_SECRET`
                        ConsentType = testEnvironment ? OpenIddictConstants.ConsentTypes.Systematic : OpenIddictConstants.ConsentTypes.Explicit,
                        DisplayName = "Metabase client application",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "Metabase-Klient-Anwendung"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri(testEnvironment ? "urn:test" : "https://metabase.org:4041/logout-callback") // TODO Use correct URI
                        },
                        RedirectUris =
                        {
                            new Uri(testEnvironment ? "urn:test" : "https://metabase.org:4041/api/auth/callback/metabase") // TODO Use correct URI
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
                            OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.ReadApiScope,
                            OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.WriteApiScope,
                            OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.ManageUserApiScope,
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
            if (await manager.FindByNameAsync(Configuration.AuthConfiguration.ReadApiScope).ConfigureAwait(false) is null)
            {
                logger.LogDebug($"Creating scope '{Configuration.AuthConfiguration.ReadApiScope}'");
                await manager.CreateAsync(
                    new OpenIddictScopeDescriptor
                    {
                        DisplayName = "Read API access",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "API Lesezugriff"
                        },
                        Name = Configuration.AuthConfiguration.ReadApiScope,
                        Resources =
                        {
                            Configuration.AuthConfiguration.ServerName
                        }
                    }
                ).ConfigureAwait(false);
            }
            if (await manager.FindByNameAsync(Configuration.AuthConfiguration.WriteApiScope).ConfigureAwait(false) is null)
            {
                logger.LogDebug($"Creating scope '{Configuration.AuthConfiguration.WriteApiScope}'");
                await manager.CreateAsync(
                    new OpenIddictScopeDescriptor
                    {
                        DisplayName = "Write API access",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "API Schreibzugriff"
                        },
                        Name = Configuration.AuthConfiguration.WriteApiScope,
                        Resources =
                        {
                            Configuration.AuthConfiguration.ServerName
                        }
                    }
                ).ConfigureAwait(false);
            }
        }
    }
}