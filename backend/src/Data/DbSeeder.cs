using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Metabase.Authentication;
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
        Level = LogLevel.Debug,
        Message = "Seeding the database")]
    public static partial void SeedingDatabase(
        this ILogger<DbSeeder> logger
    );

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Creating role {Role}")]
    public static partial void CreatingRole(
        this ILogger<DbSeeder> logger,
        Enumerations.UserRole role
    );

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Creating user {Name}")]
    public static partial void CreatingUser(
        this ILogger<DbSeeder> logger,
        string name
    );

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Creating application client '{ClientId}'")]
    public static partial void CreatingApplicationClient(
        this ILogger<DbSeeder> logger,
        string clientId
    );

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Creating scope '{Scope}'")]
    public static partial void CreatingScope(
        this ILogger<DbSeeder> logger,
        string scope
    );
}

public sealed class DbSeeder
{
    public static readonly ReadOnlyCollection<(string Name, string EmailAddress, Enumerations.UserRole Role)> Users =
        Role.AllEnum.Select(role => (
            Role.EnumToName(role),
            $"{Role.EnumToName(role).ToLowerInvariant()}@buildingenvelopedata.org",
            role
        )).ToList().AsReadOnly();

    public static readonly (string Name, string EmailAddress, Enumerations.UserRole Role)
        AdministratorUser =
            Users.First(_ => _.Role == Enumerations.UserRole.ADMINISTRATOR);

    public static readonly (string Name, string EmailAddress, Enumerations.UserRole Role)
        VerifierUser =
            Users.First(_ => _.Role == Enumerations.UserRole.VERIFIER);

    public static readonly (string Name, string EmailAddress, Enumerations.UserRole Role)
        SupporterUser =
            Users.First(_ => _.Role == Enumerations.UserRole.SUPPORTER);

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
            if ((await manager.GetUsersInRoleAsync(Role.Administrator)).Count is 0)
            {
                await CreateUserAsync(manager, AdministratorUser, appSettings.BootstrapUserPassword, logger);
            }
        }
        else
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            foreach (var userInfo in Users)
            {
                if (await manager.FindByEmailAsync(userInfo.EmailAddress) is null)
                {
                    await CreateUserAsync(
                        manager,
                        userInfo,
                        appSettings.BootstrapUserPassword,
                        logger
                    );
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
        var iseInstitution = await context.Institutions.Where(_ => _.Id == new Guid(DataConstants.IseInstitutionUuid)).SingleOrDefaultAsync();
        if (iseInstitution is null)
        {
            iseInstitution = new Institution(
                new Guid(DataConstants.IseInstitutionUuid),
                "Fraunhofer ISE",
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
                    UserId = (await context.Users.Where(_ => _.Email == AdministratorUser.EmailAddress).SingleAsync()).Id,
                    Role = InstitutionRepresentativeRole.OWNER,
                    Pending = false
                }
            );
            var application = await manager.FindByClientIdAsync(OpenIdConnectConstants.Client.MetabaseClientId).AsTask();
            if (application is not null)
            {
                iseInstitution.OpenIdConnectApplications.Add(application);
            }
            context.Institutions.Add(iseInstitution);
        }
        if (!await context.Institutions.Where(_ => _.Id == new Guid(DataConstants.TestlabInstitutionUuid)).AnyAsync())
        {
            var institution = new Institution(
                new Guid(DataConstants.TestlabInstitutionUuid),
                "TestLab Solar Facades",
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
            var application = await manager.FindByClientIdAsync(DataConstants.TestlabOpenIdConnectClientId).AsTask();
            if (application is not null)
            {
                institution.OpenIdConnectApplications.Add(application);
            }
            context.Institutions.Add(institution);
        }
        if (!await context.Institutions.Where(_ => _.Id == new Guid(DataConstants.LbnlInstitutionUuid)).AnyAsync())
        {
            var institution = new Institution(
                new Guid(DataConstants.LbnlInstitutionUuid),
                "LBNL",
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
        }
        if (!await context.Institutions.Where(_ => _.Id == new Guid(DataConstants.EpeaInstitutionUuid)).AnyAsync())
        {
            var institution = new Institution(
                new Guid(DataConstants.EpeaInstitutionUuid),
                "EPEA - Part of Drees & Sommer",
                "EPEA",
                "Sustainability",
                new ContactInformation(
                    phoneNumber: null,
                    isPhoneNumberConfirmed: true,
                    postalAddress: null,
                    emailAddress: null,
                    isEmailAddressConfirmed: false,
                    websiteLocator: null
                ),
                InstitutionState.VERIFIED,
                InstitutionOperatingState.OPERATING,
                null
            )
            {
                ManagerId = iseInstitution.Id
            };
            context.Institutions.Add(institution);
        }
        await context.SaveChangesAsync();
    }

    private static async Task CreateDatabasesAsync(
        IServiceProvider services,
        IWebHostEnvironment environment,
        AppSettings appSettings
    )
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (!await context.Databases.Where(_ => _.Id == new Guid(DataConstants.TestlabDatabaseUuid)).AnyAsync())
        {
            var uriBuilder = new UriBuilder(appSettings.TestlabSolarFacades.Uri)
            {
                Path = "/graphql/"
            };
            var database = new Database(
                new Guid(DataConstants.TestlabDatabaseUuid),
                "TestLab DB",
                "The database of the TestLab Solar Facades of Fraunhofer ISE",
                uriBuilder.Uri
            )
            {
                OperatorId = new Guid(DataConstants.TestlabInstitutionUuid)
            };
            database.Verify();
            context.Databases.Add(database);
        }
        if (!await context.Databases.Where(_ => _.Id == new Guid(DataConstants.IgsdbDatabaseUuid)).AnyAsync())
        {
            var uriBuilder = new UriBuilder(new Uri("https://igsdb-v2-staging.herokuapp.com", UriKind.Absolute))
            {
                Path = "/graphql/"
            };
            var database = new Database(
                new Guid(DataConstants.IgsdbDatabaseUuid),
                "IGSDB",
                "The International Glazing and Shading Database (IGSDB)",
                uriBuilder.Uri
            )
            {
                OperatorId = new Guid(DataConstants.LbnlInstitutionUuid)
            };
            database.Verify();
            context.Databases.Add(database);
        }
        if (!await context.Databases.Where(_ => _.Id == new Guid(DataConstants.EpeaDatabaseUuid)).AnyAsync())
        {
            var uriBuilder = new UriBuilder(new Uri("https://app.conpli.eu/GraphQL", UriKind.Absolute))
            {
                Path = "/graphql/"
            };
            var database = new Database(
                new Guid(DataConstants.EpeaDatabaseUuid),
                "ProCA Database",
                "Database for Life-Cycle data of components",
                uriBuilder.Uri
            )
            {
                OperatorId = new Guid(DataConstants.EpeaInstitutionUuid)
            };
            database.Verify();
            context.Databases.Add(database);
        }
        await context.SaveChangesAsync();
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
                    }
                }
            );
        }

        if (await manager.FindByNameAsync(OpenIdConnectScope.SupportApiScope) is null)
        {
            logger.CreatingScope(OpenIdConnectScope.SupportApiScope);
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor
                {
                    DisplayName = "Allow customer support role",
                    Name = OpenIdConnectScope.SupportApiScope,
                    Resources =
                    {
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
                        OpenIdConnectConstants.Client.MetabaseClientId
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
        if (await manager.FindByClientIdAsync(OpenIdConnectConstants.Client.MetabaseClientId) is null)
        {
            logger.CreatingApplicationClient(OpenIdConnectConstants.Client.MetabaseClientId);
            var host = appSettings.Uri;
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = OpenIdConnectConstants.Client.MetabaseClientId,
                ClientSecret = null,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Metabase",
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
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Token,
                },
                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                    OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests,
                }
            }
            .AddGrantTypePermissions(
                environment.IsEnvironment(Program.TestEnvironment)
                ? OpenIddictConstants.GrantTypes.Password
                : OpenIddictConstants.GrantTypes.AuthorizationCode,
                OpenIddictConstants.GrantTypes.ClientCredentials,
                OpenIddictConstants.GrantTypes.RefreshToken,
                OpenIddictConstants.GrantTypes.TokenExchange
            )
            .AddScopePermissions(OpenIdConnectScope.Scopes)
            .AddAudiencePermissions(OpenIdConnectConstants.Client.MetabaseClientId)
            .AddResourcePermissions(appSettings.GraphQlEndpoint.AbsoluteUri);
            var application = new OpenIdConnectApplication
            {
                OwnerId = new Guid(DataConstants.IseInstitutionUuid)
            };
            await manager.PopulateAsync(application, descriptor);
            // The secret is used in tests, see `IntegrationTests#RequestAuthToken` and in
            // the metabase client, see `OPEN_ID_CONNECT_CLIENT_SECRET` in `.env.*`.
            await manager.CreateAsync(application, appSettings.OpenIdConnectClientSecret);
        }
        if (await manager.FindByClientIdAsync(DataConstants.TestlabOpenIdConnectClientId) is null)
        {
            logger.CreatingApplicationClient(DataConstants.TestlabOpenIdConnectClientId);
            var host = appSettings.TestlabSolarFacades.Uri;
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = DataConstants.TestlabOpenIdConnectClientId,
                ClientSecret = null,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "TestLab Solar Façades",
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
                        OpenIddictConstants.Permissions.Endpoints.EndSession,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,
                    },
                Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                        OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests
                    }
            }
            .AddGrantTypePermissions(
                OpenIddictConstants.GrantTypes.AuthorizationCode,
                OpenIddictConstants.GrantTypes.RefreshToken
            )
            .AddScopePermissions(
                OpenIddictConstants.Scopes.Profile,
                OpenIdConnectScope.ReadApiScope,
                OpenIdConnectScope.WriteApiScope,
                OpenIdConnectScope.ManageDatabaseApiScope
            )
            .AddAudiencePermissions(OpenIdConnectConstants.Client.MetabaseClientId);
            var application = new OpenIdConnectApplication
            {
                OwnerId = new Guid(DataConstants.TestlabInstitutionUuid)
            };
            await manager.PopulateAsync(application, descriptor);
            // The secret is used in the database client, see
            // `OPEN_ID_CONNECT_CLIENT_SECRET` in `.env.*`.
            await manager.CreateAsync(application, appSettings.TestlabSolarFacades.OpenIdConnectClientSecret);
        }
        if (await manager.FindByClientIdAsync(DataConstants.IgsdbOpenIdConnectClientId) is null)
        {
            logger.CreatingApplicationClient(DataConstants.IgsdbOpenIdConnectClientId);
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = DataConstants.IgsdbOpenIdConnectClientId,
                ClientSecret = null,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "IGSDB",
                RedirectUris = { },
                PostLogoutRedirectUris = { },
                Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.EndSession,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.ResponseTypes.Token,
                    },
                Requirements =
                    {
                        OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                        OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests
                    }
            }
            .AddGrantTypePermissions(
                OpenIddictConstants.GrantTypes.ClientCredentials,
                OpenIddictConstants.GrantTypes.RefreshToken
            )
            .AddScopePermissions(
                OpenIdConnectScope.ReadApiScope,
                OpenIdConnectScope.WriteApiScope
            )
            .AddAudiencePermissions(OpenIdConnectConstants.Client.MetabaseClientId);
            var application = new OpenIdConnectApplication
            {
                OwnerId = new Guid(DataConstants.LbnlInstitutionUuid)
            };
            await manager.PopulateAsync(application, descriptor);
            await manager.CreateAsync(application, appSettings.Igsdb.OpenIdConnectClientSecret);
        }
    }
}