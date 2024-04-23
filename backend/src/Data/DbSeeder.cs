using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.ObjectModel;

namespace Metabase.Data;

public static partial class Log
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Debug,
        Message = "Seeding the database")]
    public static partial void SeedingDatabase(
        this ILogger logger
    );

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Creating role {Role}")]
    public static partial void CreatingRole(
        this ILogger logger,
        Enumerations.UserRole role
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Creating user {Name}")]
    public static partial void CreatingUser(
        this ILogger logger,
        string name
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Creating application client '{ClientId}'")]
    public static partial void CreatingApplicationClient(
        this ILogger logger,
        string clientId
    );

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Creating scope '{Scope}'")]
    public static partial void CreatingScope(
        this ILogger logger,
        string scope
    );
}

public sealed class DbSeeder
{
    public static readonly ReadOnlyCollection<(string Name, string EmailAddress, string Password,
        Enumerations.UserRole Role)> Users =
        Role.AllEnum.Select(role => (
            Role.EnumToName(role),
            $"{Role.EnumToName(role).ToLowerInvariant()}@buildingenvelopedata.org",
            "abcABC123@",
            role
        )).ToList().AsReadOnly();

    public static readonly (string Name, string EmailAddress, string Password, Enumerations.UserRole Role)
        AdministratorUser =
            Users.First(x => x.Role == Enumerations.UserRole.ADMINISTRATOR);

    public static readonly (string Name, string EmailAddress, string Password, Enumerations.UserRole Role)
        VerifierUser =
            Users.First(x => x.Role == Enumerations.UserRole.VERIFIER);

    public const string MetabaseClientId = "metabase";
    public const string TestlabSolarFacadesClientId = "testlab-solar-facades";

    public static async Task DoAsync(
        IServiceProvider services
    )
    {
        var logger = services.GetRequiredService<ILogger<DbSeeder>>();
        logger.SeedingDatabase();
        var environment = services.GetRequiredService<IWebHostEnvironment>();
        var appSettings = services.GetRequiredService<AppSettings>();
        await CreateRolesAsync(services, logger).ConfigureAwait(false);
        await CreateUsersAsync(services, logger).ConfigureAwait(false);
        await RegisterApplicationsAsync(services, logger, environment, appSettings).ConfigureAwait(false);
        await RegisterScopesAsync(services, logger).ConfigureAwait(false);
    }

    private static async Task CreateRolesAsync(
        IServiceProvider services,
        ILogger<DbSeeder> logger
    )
    {
        var manager = services.GetRequiredService<RoleManager<Role>>();
        foreach (var role in Role.AllEnum)
            if (await manager.FindByNameAsync(Role.EnumToName(role)).ConfigureAwait(false) is null)
            {
                logger.CreatingRole(role);
                await manager.CreateAsync(
                    new Role(role)
                ).ConfigureAwait(false);
            }
    }

    private static async Task CreateUsersAsync(
        IServiceProvider services,
        ILogger<DbSeeder> logger
    )
    {
        var manager = services.GetRequiredService<UserManager<User>>();
        foreach (var (Name, EmailAddress, Password, Role) in Users)
            if (await manager.FindByNameAsync(Name).ConfigureAwait(false) is null)
            {
                logger.CreatingUser(Name);
                var user = new User(Name, EmailAddress, null, null);
                await manager.CreateAsync(
                    user,
                    Password
                ).ConfigureAwait(false);
                var confirmationToken =
                    await manager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                await manager.ConfirmEmailAsync(user, confirmationToken).ConfigureAwait(false);
                await manager.AddToRoleAsync(user, Data.Role.EnumToName(Role)).ConfigureAwait(false);
            }
    }

    private static async Task RegisterApplicationsAsync(
        IServiceProvider services,
        ILogger<DbSeeder> logger,
        IWebHostEnvironment environment,
        AppSettings appSettings
    )
    {
        var manager = services.GetRequiredService<IOpenIddictApplicationManager>();
        if (await manager.FindByClientIdAsync(MetabaseClientId).ConfigureAwait(false) is null)
        {
            logger.CreatingApplicationClient(MetabaseClientId);
            var host = appSettings.Host;
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = MetabaseClientId,
                    // The secret is used in tests, see
                    // `IntegrationTests#RequestAuthToken` and in the
                    // metabase client, see `OPEN_ID_CONNECT_CLIENT_SECRET`
                    // in `.env.*`.
                    ClientSecret = appSettings.OpenIdConnectClientSecret,
                    ConsentType = environment.IsEnvironment("test")
                        ? OpenIddictConstants.ConsentTypes.Systematic
                        : OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Metabase client application",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("de-DE")] = "Metabase-Klient-Anwendung"
                    },
                    RedirectUris =
                    {
                        new Uri(environment.IsEnvironment("test")
                            ? "urn:test"
                            : $"{host}/connect/callback/login/metabase")
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri(environment.IsEnvironment("test")
                            ? "urn:test"
                            : $"{host}/connect/callback/logout/metabase")
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        // OpenIddictConstants.Permissions.Endpoints.Device,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        environment.IsEnvironment("test")
                            ? OpenIddictConstants.Permissions.GrantTypes.Password
                            : OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        // OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        // OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        environment.IsEnvironment("test")
                            ? OpenIddictConstants.Permissions.ResponseTypes.Token
                            : OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        Configuration.AuthConfiguration.ReadApiScope,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        Configuration.AuthConfiguration.WriteApiScope,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        Configuration.AuthConfiguration.ManageUserApiScope
                    },
                    Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                    }
                }
            ).ConfigureAwait(false);
        }

        if (await manager.FindByClientIdAsync(TestlabSolarFacadesClientId).ConfigureAwait(false) is null)
        {
            logger.CreatingApplicationClient(TestlabSolarFacadesClientId);
            var host = appSettings.TestlabSolarFacadesHost;
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = TestlabSolarFacadesClientId,
                    // The secret is used in the database client, see
                    // `OPEN_ID_CONNECT_CLIENT_SECRET` in `.env.*`.
                    ClientSecret = appSettings.TestlabSolarFacadesOpenIdConnectClientSecret,
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Testlab-Solar-Facades client application",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("de-DE")] = "Testlab-Solar-Facades-Klient-Anwendung"
                    },
                    RedirectUris =
                    {
                        new Uri($"{host}/connect/callback/login/metabase")
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri($"{host}/connect/callback/logout/metabase")
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        // OpenIddictConstants.Permissions.Endpoints.Device,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        // OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        // OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        Configuration.AuthConfiguration.ReadApiScope,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        Configuration.AuthConfiguration.WriteApiScope
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
        if (await manager.FindByNameAsync(Configuration.AuthConfiguration.ReadApiScope)
                .ConfigureAwait(false) is null)
        {
            logger.CreatingScope(Configuration.AuthConfiguration.ReadApiScope);
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
                        Configuration.AuthConfiguration.Audience
                    }
                }
            ).ConfigureAwait(false);
        }

        if (await manager.FindByNameAsync(Configuration.AuthConfiguration.WriteApiScope)
                .ConfigureAwait(false) is null)
        {
            logger.CreatingScope(Configuration.AuthConfiguration.WriteApiScope);
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
                        Configuration.AuthConfiguration.Audience
                    }
                }
            ).ConfigureAwait(false);
        }

        if (await manager.FindByNameAsync(Configuration.AuthConfiguration.ManageUserApiScope)
                .ConfigureAwait(false) is null)
        {
            logger.CreatingScope(Configuration.AuthConfiguration.ManageUserApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Manage user API access",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("de-DE")] = "Benutzerverwaltung-API-Zugriff"
                    },
                    Name = Configuration.AuthConfiguration.ManageUserApiScope,
                    Resources =
                    {
                        Configuration.AuthConfiguration.Audience
                    }
                }
            ).ConfigureAwait(false);
        }
    }
}