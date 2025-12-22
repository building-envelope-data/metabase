using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Metabase.Configuration;
using Metabase.Data.OpenIdConnect;
using Metabase.Enumerations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Core;

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
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Creating scope '{Scope}'")]
    public static partial void CreatingScope(
        this ILogger logger,
        string scope
    );
}

public sealed class DbSeeder
{
    public const string TestlabSolarFacadesOpenIdConnectClientId = "testlab-solar-facades";
    public const string IgsdbOpenIdConnectClientId = "igsdb";

    public static readonly ReadOnlyCollection<(string Name, string EmailAddress, Enumerations.UserRole Role)> Users =
        Role.AllEnum.Select(role => (
            Role.EnumToName(role),
            $"{Role.EnumToName(role).ToLowerInvariant()}@buildingenvelopedata.org",
            role
        )).ToList().AsReadOnly();

    public static readonly (string Name, string EmailAddress, Enumerations.UserRole Role)
        AdministratorUser =
            Users.First(x => x.Role == Enumerations.UserRole.ADMINISTRATOR);

    public static readonly (string Name, string EmailAddress, Enumerations.UserRole Role)
        VerifierUser =
            Users.First(x => x.Role == Enumerations.UserRole.VERIFIER);

    private const string IseInstitutionName = "Fraunhofer ISE";
    private const string TestlabInstitutionName = "TestLab Solar Facades";
    private const string LbnlInstitutionName = "LBNL";

    private const string TestlabDatabaseName = "TestLab DB";
    private const string IgsdbDatabaseName = "IGSDB";

    public static async Task DoAsync(
        IServiceProvider services
    )
    {
        var logger = services.GetRequiredService<ILogger<DbSeeder>>();
        logger.SeedingDatabase();
        var environment = services.GetRequiredService<IWebHostEnvironment>();
        var appSettings = services.GetRequiredService<AppSettings>();
        await CreateRolesAsync(services, logger);
        await CreateUsersAsync(services, environment, appSettings, logger);
        await CreateInstitutionsAsync(services, environment);
        await CreateDatabasesAsync(services, environment, appSettings);
        await CreateOpenIdConnectScopes(services, logger);
        await CreateOpenIdConnectApplications(services, logger, environment, appSettings);
    }

    private static async Task CreateRolesAsync(
        IServiceProvider services,
        ILogger<DbSeeder> logger
    )
    {
        var manager = services.GetRequiredService<RoleManager<Role>>();
        foreach (var role in Role.AllEnum)
        {
            if (await manager.FindByNameAsync(Role.EnumToName(role)) is null)
            {
                logger.CreatingRole(role);
                await manager.CreateAsync(
                    new Role(role)
                );
            }
        }
    }

    private static async Task CreateUsersAsync(
        IServiceProvider services,
        IWebHostEnvironment environment,
        AppSettings appSettings,
        ILogger<DbSeeder> logger
    )
    {
        var manager = services.GetRequiredService<UserManager<User>>();
        if (environment.IsProduction())
        {
            if ((await manager.GetUsersInRoleAsync(Role.Administrator)).Count == 0)
            {
                await CreateUserAsync(manager, AdministratorUser, appSettings.BootstrapUserPassword, logger);
            }
        }
        else
        {
            foreach (var userInfo in Users)
            {
                if (await manager.FindByEmailAsync(userInfo.EmailAddress) is null)
                {
                    await CreateUserAsync(manager, userInfo, appSettings.BootstrapUserPassword, logger);
                }
            }
        }
    }

    private static async Task CreateUserAsync(
        UserManager<User> manager,
        (string Name, string EmailAddress, Enumerations.UserRole Role) userInfo,
        string password,
        ILogger<DbSeeder> logger
    )
    {
        logger.CreatingUser(userInfo.Name);
        var user = new User(userInfo.Name, userInfo.EmailAddress, null, null);
        await manager.CreateAsync(
            user,
            password
        );
        var confirmationToken =
            await manager.GenerateEmailConfirmationTokenAsync(user);
        await manager.ConfirmEmailAsync(user, confirmationToken);
        await manager.AddToRoleAsync(user, Role.EnumToName(userInfo.Role));
    }

    private static async Task CreateInstitutionsAsync(
        IServiceProvider services,
        IWebHostEnvironment environment
    )
    {
        var manager = services.GetRequiredService<OpenIddictApplicationManager<OpenIdConnectApplication>>();
        var context = services.GetRequiredService<ApplicationDbContext>();
        var iseInstitution = await context.Institutions.Where(x => x.Name == IseInstitutionName).SingleOrDefaultAsync();
        if (iseInstitution is null)
        {
            iseInstitution = new Institution(
                IseInstitutionName,
                "ISE",
                "Fraunhofer Institute for Solar Energy Systems (ISE)",
                new ContactInformation(
                    phoneNumber: "+49 761 45880",
                    isPhoneNumberConfirmed: true,
                    postalAddress: "Heidenhofstraße 2, 79110 Freiburg im Breisgau",
                    emailAddress: null,
                    isEmailAddressConfirmed: false,
                    websiteLocator: new Uri("https://www.ise.fraunhofer.de", UriKind.Absolute)
                ),
                InstitutionState.VERIFIED,
                InstitutionOperatingState.OPERATING,
                null
            );
            iseInstitution.RepresentativeEdges.Add(
                new InstitutionRepresentative
                {
                    UserId = (await context.Users.Where(x => x.Email == AdministratorUser.EmailAddress).SingleAsync()).Id,
                    Role = InstitutionRepresentativeRole.OWNER,
                    Pending = false
                }
            );
            var application = await manager.FindByClientIdAsync(AuthConfiguration.MetabaseOpenIdConnectClientId).AsTask();
            if (application is not null)
            {
                iseInstitution.OpenIdConnectApplications.Add(application);
            }
            context.Institutions.Add(iseInstitution);
            await context.SaveChangesAsync();
        }
        if (environment.IsDevelopment())
        {
            if (!await context.Institutions.Where(x => x.Name == TestlabInstitutionName).AnyAsync())
            {
                var institution = new Institution(
                    TestlabInstitutionName,
                    "TLSF",
                    "This institution represents the TestLab Solar Facades of Fraunhofer ISE",
                    new ContactInformation(
                        phoneNumber: "+49 761 4588-5673",
                        isPhoneNumberConfirmed: true,
                        postalAddress: "Heidenhofstraße 2, 79110 Freiburg im Breisgau",
                        emailAddress: null,
                        isEmailAddressConfirmed: false,
                        websiteLocator: new Uri("https://www.ise.fraunhofer.de/en/rd-infrastructure/accredited-labs/testlab-solar-facades.html", UriKind.Absolute)
                    ),
                    InstitutionState.VERIFIED,
                    InstitutionOperatingState.OPERATING,
                    null
                )
                {
                    ManagerId = iseInstitution.Id
                };

                var application = await manager.FindByClientIdAsync(TestlabSolarFacadesOpenIdConnectClientId).AsTask();
                if (application is not null)
                {
                    institution.OpenIdConnectApplications.Add(application);
                }
                context.Institutions.Add(institution);
                await context.SaveChangesAsync();
            }
            if (!await context.Institutions.Where(x => x.Name == LbnlInstitutionName).AnyAsync())
            {
                var institution = new Institution(
                    LbnlInstitutionName,
                    "LBNL",
                    "Lawrence Berkeley National Laboratory",
                    new ContactInformation(
                        phoneNumber: "(510) 486-4000",
                        isPhoneNumberConfirmed: true,
                        postalAddress: "1 Cyclotron Road, Berkeley, CA 94720",
                        emailAddress: null,
                        isEmailAddressConfirmed: false,
                        websiteLocator: new Uri("https://www.lbl.gov", UriKind.Absolute)
                    ),
                    InstitutionState.VERIFIED,
                    InstitutionOperatingState.OPERATING,
                    null
                )
                {
                    ManagerId = iseInstitution.Id
                };
                context.Institutions.Add(institution);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task CreateDatabasesAsync(
        IServiceProvider services,
        IWebHostEnvironment environment,
        AppSettings appSettings
    )
    {
        if (environment.IsDevelopment())
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            if (!await context.Databases.Where(x => x.Name == TestlabDatabaseName).AnyAsync())
            {
                var uriBuilder = new UriBuilder(appSettings.TestlabSolarFacadesHostUri)
                {
                    Path = "/graphql/"
                };
                var database = new Database(
                    TestlabDatabaseName,
                    "The database of the TestLab Solar Facades of Fraunhofer ISE",
                    uriBuilder.Uri
                )
                {
                    OperatorId = (await context.Institutions.SingleAsync(x => x.Name == TestlabInstitutionName)).Id
                };
                database.Verify();
                context.Databases.Add(database);
                await context.SaveChangesAsync();
            }
            if (!await context.Databases.Where(x => x.Name == IgsdbDatabaseName).AnyAsync())
            {
                var uriBuilder = new UriBuilder(new Uri("https://igsdb-v2-staging.herokuapp.com", UriKind.Absolute))
                {
                    Path = "/graphql/"
                };
                var database = new Database(
                    IgsdbDatabaseName,
                    "The International Glazing and Shading Database (IGSDB)",
                    uriBuilder.Uri
                )
                {
                    OperatorId = (await context.Institutions.SingleAsync(x => x.Name == LbnlInstitutionName)).Id
                };
                database.Verify();
                context.Databases.Add(database);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task CreateOpenIdConnectScopes(
        IServiceProvider services,
        ILogger<DbSeeder> logger
    )
    {
        var manager = services.GetRequiredService<OpenIddictScopeManager<OpenIdConnectScope>>();
        if (await manager.FindByNameAsync(OpenIdConnectScope.ReadApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.ReadApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Read API access",
                    Name = OpenIdConnectScope.ReadApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.WriteApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.WriteApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Write API access",
                    Name = OpenIdConnectScope.WriteApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.AdministrateApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.AdministrateApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Allow administrator role",
                    Name = OpenIdConnectScope.AdministrateApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.VerifyApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.VerifyApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Allow verifier role",
                    Name = OpenIdConnectScope.VerifyApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.ManageUserApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.ManageUserApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Manage users",
                    Name = OpenIdConnectScope.ManageUserApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.ManageOpenIdConnectApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.ManageOpenIdConnectApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Manage OpenId Connect configuration",
                    Name = OpenIdConnectScope.ManageOpenIdConnectApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.ManageInstitutionRepresentativeApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.ManageInstitutionRepresentativeApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Manage institution representatives",
                    Name = OpenIdConnectScope.ManageInstitutionRepresentativeApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.ManageGnuPgApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.ManageGnuPgApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Manage GnuPG configuration",
                    Name = OpenIdConnectScope.ManageGnuPgApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.ManageDatabaseApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.ManageDatabaseApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Manage databases",
                    Name = OpenIdConnectScope.ManageDatabaseApiScope,
                    Resources =
                    {
                        AuthConfiguration.MetabaseOpenIdConnectClientId
                    }
                }
            );
        }
    }

    private static async Task CreateOpenIdConnectApplications(
        IServiceProvider services,
        ILogger<DbSeeder> logger,
        IWebHostEnvironment environment,
        AppSettings appSettings
    )
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var manager = services.GetRequiredService<OpenIddictApplicationManager<OpenIdConnectApplication>>();
        if (await manager.FindByClientIdAsync(AuthConfiguration.MetabaseOpenIdConnectClientId) is null)
        {
            logger.CreatingApplicationClient(AuthConfiguration.MetabaseOpenIdConnectClientId);
            var host = appSettings.HostUri;
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = AuthConfiguration.MetabaseOpenIdConnectClientId,
                ClientSecret = null,
                ConsentType = environment.IsEnvironment(Program.TestEnvironment)
                        ? OpenIddictConstants.ConsentTypes.Systematic
                        : OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Metabase client application",
                RedirectUris =
                {
                    environment.IsEnvironment(Program.TestEnvironment)
                    ? new Uri("urn:test", UriKind.Absolute)
                    : new UriBuilder(host) { Path = "/connect/callback/login/metabase" }.Uri
                },
                PostLogoutRedirectUris =
                {
                    environment.IsEnvironment(Program.TestEnvironment)
                    ? new Uri("urn:test", UriKind.Absolute)
                    : new UriBuilder(host) { Path = "/connect/callback/logout/metabase" }.Uri
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    environment.IsEnvironment(Program.TestEnvironment)
                        ? OpenIddictConstants.Permissions.GrantTypes.Password
                        : OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    environment.IsEnvironment(Program.TestEnvironment)
                        ? OpenIddictConstants.Permissions.ResponseTypes.Token
                        : OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.ReadApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.WriteApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.AdministrateApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.VerifyApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.ManageUserApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.ManageOpenIdConnectApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.ManageInstitutionRepresentativeApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.ManageGnuPgApiScope,
                    OpenIddictConstants.Permissions.Prefixes.Scope +
                    OpenIdConnectScope.ManageDatabaseApiScope
                },
                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                    OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests
                }
            };
            var application = new OpenIdConnectApplication
            {
                OwnerId = (await context.Institutions.SingleAsync(x => x.Name == IseInstitutionName)).Id
            };
            await manager.PopulateAsync(application, descriptor);
            // The secret is used in tests, see `IntegrationTests#RequestAuthToken` and in
            // the metabase client, see `OPEN_ID_CONNECT_CLIENT_SECRET` in `.env.*`.
            await manager.CreateAsync(application, appSettings.OpenIdConnectClientSecret);
        }

        if (environment.IsDevelopment())
        {
            if (await manager.FindByClientIdAsync(TestlabSolarFacadesOpenIdConnectClientId) is null)
            {
                logger.CreatingApplicationClient(TestlabSolarFacadesOpenIdConnectClientId);
                var host = appSettings.TestlabSolarFacadesHostUri;
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = TestlabSolarFacadesOpenIdConnectClientId,
                    ClientSecret = null,
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Testlab-Solar-Facades client application",
                    RedirectUris =
                    {
                        new UriBuilder(host) { Path = "/connect/callback/login/metabase" }.Uri
                    },
                    PostLogoutRedirectUris =
                    {
                        new UriBuilder(host) { Path = "/connect/callback/logout/metabase" }.Uri
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.GrantTypes.TokenExchange,
                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        OpenIdConnectScope.ReadApiScope,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        OpenIdConnectScope.WriteApiScope
                    },
                    Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                        OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests
                    }
                };
                var application = new OpenIdConnectApplication
                {
                    OwnerId = (await context.Institutions.SingleAsync(x => x.Name == TestlabInstitutionName)).Id
                };
                await manager.PopulateAsync(application, descriptor);
                // The secret is used in the database client, see
                // `OPEN_ID_CONNECT_CLIENT_SECRET` in `.env.*`.
                await manager.CreateAsync(application, appSettings.TestlabSolarFacadesOpenIdConnectClientSecret);
            }

            if (await manager.FindByClientIdAsync(IgsdbOpenIdConnectClientId) is null)
            {
                logger.CreatingApplicationClient(IgsdbOpenIdConnectClientId);
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = IgsdbOpenIdConnectClientId,
                    ClientSecret = null,
                    ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                    DisplayName = "IGSDB client application",
                    RedirectUris = { },
                    PostLogoutRedirectUris = { },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,
                        OpenIddictConstants.Permissions.Scopes.Address,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Phone,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        OpenIdConnectScope.ReadApiScope,
                        OpenIddictConstants.Permissions.Prefixes.Scope +
                        OpenIdConnectScope.WriteApiScope
                    },
                    Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                        OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests
                    }
                };
                var application = new OpenIdConnectApplication
                {
                    OwnerId = (await context.Institutions.SingleAsync(x => x.Name == LbnlInstitutionName)).Id
                };
                await manager.PopulateAsync(application, descriptor);
                await manager.CreateAsync(application, appSettings.IgsdbOpenIdConnectClientSecret);
            }
        }
    }
}