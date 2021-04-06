using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using Microsoft.Extensions.Hosting;

namespace Metabase.Data
{
    public sealed class DbSeeder
    {
        public static async Task DoAsync(
            IServiceProvider services
            )
        {
            var logger = services.GetRequiredService<ILogger<DbSeeder>>();
            logger.LogDebug("Seeding the database");
            var environment = services.GetRequiredService<IWebHostEnvironment>();
            var appSettings = services.GetRequiredService<AppSettings>();
            await RegisterApplicationsAsync(services, logger, environment, appSettings).ConfigureAwait(false);
            await RegisterScopesAsync(services, logger, appSettings).ConfigureAwait(false);
        }

        private static async Task RegisterApplicationsAsync(
            IServiceProvider services,
            ILogger<DbSeeder> logger,
            IWebHostEnvironment environment,
            AppSettings appSettings
            )
        {
            var manager = services.GetRequiredService<IOpenIddictApplicationManager>();
            if (await manager.FindByClientIdAsync("testlab-solar-facades").ConfigureAwait(false) is null)
            {
                logger.LogDebug("Creating application client 'testlab-solar-facades'");
                var host = environment.IsProduction() ? "https://testlab-solar-facades.de" : "https://local.testlab-solar-facades.de:5051";
                await manager.CreateAsync(
                    new OpenIddictApplicationDescriptor
                    {
                        ClientId = "testlab-solar-facades",
                        // The secret is used in tests, see `IntegrationTests#RequestAuthToken` and in the database frontned, see `AUTH_CLIENT_SECRET` in `/frontend/.env.*`.
                        ClientSecret = appSettings.TestlabSolarFacadesOpenIdConnectClientSecret,
                        ConsentType = environment.IsEnvironment("test") ? OpenIddictConstants.ConsentTypes.Systematic : OpenIddictConstants.ConsentTypes.Explicit,
                        DisplayName = "Testlab-Solar-Facades client application",
                        DisplayNames =
                        {
                            [CultureInfo.GetCultureInfo("de-DE")] = "Testlab-Solar-Facades-Klient-Anwendung"
                        },
                        PostLogoutRedirectUris =
                        {
                            new Uri(environment.IsEnvironment("test") ? "urn:test" : $"{host}/users/login")
                        },
                        RedirectUris =
                        {
                            new Uri(environment.IsEnvironment("test") ? "urn:test" : $"{host}/api/auth/callback/metabase")
                        },
                        Permissions = {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            // OpenIddictConstants.Permissions.Endpoints.Device,
                            // OpenIddictConstants.Permissions.Endpoints.Introspection,
                            OpenIddictConstants.Permissions.Endpoints.Logout,
                            // OpenIddictConstants.Permissions.Endpoints.Revocation,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            environment.IsEnvironment("test") ? OpenIddictConstants.Permissions.GrantTypes.Password : OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            // OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                            // OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            environment.IsEnvironment("test") ? OpenIddictConstants.Permissions.ResponseTypes.Token : OpenIddictConstants.Permissions.ResponseTypes.Code,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles,
                            OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.ReadApiScope,
                            OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.WriteApiScope,
                            // Is there a better way to optionally add a value to a hash set inline?
                            environment.IsEnvironment("test")
                            ? OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.ManageUserApiScope
                            : OpenIddictConstants.Permissions.Prefixes.Scope + Configuration.AuthConfiguration.ReadApiScope,
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
            ILogger<DbSeeder> logger,
            AppSettings appSettings
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
                            appSettings.Host
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
                            appSettings.Host
                        }
                    }
                ).ConfigureAwait(false);
            }
        }
    }
}