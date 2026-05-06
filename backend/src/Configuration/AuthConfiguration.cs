using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Metabase.Authentication;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Jobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using Quartz;
using Quartz.AspNetCore;

namespace Metabase.Configuration;

public static class AuthConfiguration
{
    private static readonly TimeSpan s_cookieExpirationTimeSpan = TimeSpan.FromDays(1);

    private static readonly Dictionary<string, string> s_policyNameToOpenIdConnectScope = new()
    {
        { AuthorizationPolicies.ReadScopePolicy, OpenIdConnectScope.ReadApiScope },
        { AuthorizationPolicies.WriteScopePolicy, OpenIdConnectScope.WriteApiScope },
        { AuthorizationPolicies.AdministrateScopePolicy, OpenIdConnectScope.AdministrateApiScope },
        { AuthorizationPolicies.VerifyScopePolicy, OpenIdConnectScope.VerifyApiScope },
        { AuthorizationPolicies.ManageDatabaseScopePolicy, OpenIdConnectScope.ManageDatabaseApiScope },
        { AuthorizationPolicies.ManageGnuPgScopePolicy, OpenIdConnectScope.ManageGnuPgApiScope },
        { AuthorizationPolicies.ManageInstitutionRepresentativeScopePolicy, OpenIdConnectScope.ManageInstitutionRepresentativeApiScope },
        { AuthorizationPolicies.ManageOpenIdConnectScopePolicy, OpenIdConnectScope.ManageOpenIdConnectApiScope },
        { AuthorizationPolicies.ManageUserScopePolicy, OpenIdConnectScope.ManageUserApiScope },
    };

    private static void BootstrapCertificates(IClock clock)
    {
        using var store = new X509Store(OpenIdConnectConstants.CertificateStoreName, OpenIdConnectConstants.CertificateStoreLocation);
        try
        {
            store.Open(OpenFlags.ReadWrite);
            foreach (var distinguishedName in new string[] {
                OpenIdConnectConstants.Server.SigningSubjectDistinguishedName,
                OpenIdConnectConstants.Client.SigningSubjectDistinguishedName
            })
            {
                var certificates = store.Certificates.Find(
                    X509FindType.FindBySubjectDistinguishedName,
                    distinguishedName,
                    validOnly: true
                );
                if (certificates.Count == 0)
                {
                    store.Add(
                        JwtSigningAndEncryptionCertificateRotationJob.CreateSigningCertificate(
                            distinguishedName,
                            clock
                        )
                    );
                }
            }
            foreach (var distinguishedName in new string[] {
                OpenIdConnectConstants.Server.EncryptionSubjectDistinguishedName,
                OpenIdConnectConstants.Client.EncryptionSubjectDistinguishedName
            })
            {
                var certificates = store.Certificates.Find(
                    X509FindType.FindBySubjectDistinguishedName,
                    distinguishedName,
                    validOnly: true
                );
                if (certificates.Count == 0)
                {
                    store.Add(
                        JwtSigningAndEncryptionCertificateRotationJob.CreateEncryptionCertificate(
                            distinguishedName,
                            clock
                        )
                    );
                }
            }
        }
        finally
        {
            store.Close();
        }
    }

    private static IEnumerable<X509Certificate2> FindCertificates(string distinguishedName)
    {
        using var store = new X509Store(OpenIdConnectConstants.CertificateStoreName, OpenIdConnectConstants.CertificateStoreLocation);
        try
        {
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(
                X509FindType.FindBySubjectDistinguishedName,
                distinguishedName,
                // OpenIddict automatically prioritizes the cert with the latest expiration
                // Expired keys are needed for tokens signed and encrypted before it expired
                validOnly: false
            );
            foreach (var certificate in certificates)
            {
                yield return certificate;
            }
        }
        finally
        {
            store.Close();
        }
    }

    public static void ConfigureServices(
        IServiceCollection services,
        IWebHostEnvironment environment,
        AppSettings appSettings,
        IClock clock
    )
    {
        BootstrapCertificates(clock);
        services.AddScoped<AuthenticationHandler>();
        services.AddScoped<GraphQlAuthenticationAndAntiforgeryHandler>();
        ConfigureIdentityServices(services);
        ConfigureAuthenticationAndAuthorizationServices(services);
        ConfigureTaskScheduling(services, environment);
        ConfigureOpenIddictServices(services, environment, appSettings);
        AddAuthorizationServices(services);
    }

    private static void AddAuthorizationServices(
        IServiceCollection services
    )
    {
        services.AddScoped<ApprovalAuthorization>();
        services.AddScoped<ComponentAssemblyAuthorization>();
        services.AddScoped<ComponentAuthorization>();
        services.AddScoped<ComponentGeneralizationAuthorization>();
        services.AddScoped<ComponentManufacturerAuthorization>();
        services.AddScoped<ComponentVariantAuthorization>();
        services.AddScoped<DataFormatAuthorization>();
        services.AddScoped<DatabaseAuthorization>();
        services.AddScoped<GnuPgKeyFingerprintAuthorization>();
        services.AddScoped<InstitutionAuthorization>();
        services.AddScoped<InstitutionMethodDeveloperAuthorization>();
        services.AddScoped<InstitutionRepresentativeAuthorization>();
        services.AddScoped<MethodAuthorization>();
        services.AddScoped<Authorization.OpenIdConnectAuthorization>();
        services.AddScoped<UserAuthorization>();
        services.AddScoped<UserMethodDeveloperAuthorization>();
    }

    private static void ConfigureIdentityServices(
        IServiceCollection services
    )
    {
        services.AddIdentity<User, Role>(_ =>
            {
                _.SignIn.RequireConfirmedAccount = true;
                // Password settings.
                _.Password.RequireDigit = true;
                _.Password.RequireLowercase = true;
                _.Password.RequireNonAlphanumeric = true;
                _.Password.RequireUppercase = true;
                _.Password.RequiredLength = 8;
                _.Password.RequiredUniqueChars = 1;
                // Lockout settings.
                _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(600);
                _.Lockout.MaxFailedAccessAttempts = 5;
                _.Lockout.AllowedForNewUsers = true;
                // User settings.
                _.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                _.User.RequireUniqueEmail = true;
                // Configure Identity to use the same JWT claims as OpenIddict instead of the legacy
                // WS-Federation claims it uses by default (ClaimTypes), which saves you from doing
                // the mapping in your authorization controller.
                _.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                _.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                _.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            .AddDefaultTokenProviders(); // used to generate tokens for reset passwords, change email and change telephone number operations, and for two factor authentication token generation
        // The application cookies is used by the metabase acting as authentication server through
        // the identity authentication scheme `AuthenticationConstants.IdentityApplicationScheme`.
        services.ConfigureApplicationCookie(_ =>
            {
                _.AccessDeniedPath = "/unauthorized";
                _.LoginPath = "/users/login";
                _.LogoutPath = "/me/logout";
                _.ReturnUrlParameter = "returnTo";
            }
        );
    }

    private static void ConfigureAuthenticationAndAuthorizationServices(
        IServiceCollection services
    )
    {
        // Dot not use the single authentication scheme as the default scheme https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-7.0#defaultscheme
        AppContext.SetSwitch("Microsoft.AspNetCore.Authentication.SuppressAutoDefaultScheme", true);
        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/
        services.AddAuthentication(_ =>
            {
                // Ideally, to make the various authentication control flows
                // obvious, do not use default schemes for anything by setting
                // all values below to `null` and always be explicit instead.
                // However, doing this results in an antiforgery validation
                // failing when accepting or denying on `Authorize.cshtml`. The
                // corresponding logs are
                // ```
                // Executing endpoint 'Metabase.Controllers.AuthorizationController.Accept (Metabase)'
                // Route matched with {action = "Accept", controller = "Authorization"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Accept() on controller Metabase.Controllers.AuthorizationController (Metabase).
                // Execution plan of authorization filters (in the following order): ["Microsoft.AspNetCore.Mvc.Core.Filters.AntiforgeryMiddlewareAuthorizationFilter"]
                // Execution plan of resource filters (in the following order): ["Microsoft.AspNetCore.Mvc.ViewFeatures.Filters.SaveTempDataFilter"]
                // Execution plan of action filters (in the following order): ["Microsoft.AspNetCore.Mvc.Filters.ControllerActionFilter (Order: -2147483648)", "Microsoft.AspNetCore.Mvc.ModelBinding.UnsupportedContentTypeFilter (Order: -3000)"]
                // Execution plan of exception filters (in the following order): ["None"]
                // Execution plan of result filters (in the following order): ["Microsoft.AspNetCore.Mvc.ViewFeatures.Filters.SaveTempDataFilter"]
                // Authorization Filter: Before executing OnAuthorizationAsync on filter Microsoft.AspNetCore.Mvc.Core.Filters.AntiforgeryMiddlewareAuthorizationFilter.
                // Antiforgery token validation failed. The provided antiforgery token was meant for a different claims-based user than the current user.
                // Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException: The provided antiforgery token was meant for a different claims-based user than the current user.
                //    at Microsoft.AspNetCore.Antiforgery.DefaultAntiforgery.ValidateTokens(HttpContext httpContext, AntiforgeryTokenSet antiforgeryTokenSet)
                //    at Microsoft.AspNetCore.Antiforgery.DefaultAntiforgery.ValidateRequestAsync(HttpContext httpContext)
                //    at Microsoft.AspNetCore.Antiforgery.Internal.AntiforgeryMiddleware.InvokeAwaited(HttpContext context)
                // Authorization Filter: After executing OnAuthorizationAsync on filter Microsoft.AspNetCore.Mvc.Core.Filters.AntiforgeryMiddlewareAuthorizationFilter.
                // Authorization failed for the request at filter 'Microsoft.AspNetCore.Mvc.Core.Filters.AntiforgeryMiddlewareAuthorizationFilter'.
                // Before executing action result Microsoft.AspNetCore.Mvc.AntiforgeryValidationFailedResult.
                // Executing StatusCodeResult, setting HTTP status code 400
                // After executing action result Microsoft.AspNetCore.Mvc.AntiforgeryValidationFailedResult.
                // Executed action Metabase.Controllers.AuthorizationController.Accept (Metabase) in 11.4158ms
                // Executed endpoint 'Metabase.Controllers.AuthorizationController.Accept (Metabase)'
                // HTTP POST /connect/authorize responded 400 in 190.1260 ms
                // ```
                _.DefaultAuthenticateScheme = AuthenticationConstants.IdentityApplicationScheme;
                _.DefaultChallengeScheme = AuthenticationConstants.IdentityApplicationScheme;
                _.DefaultForbidScheme = AuthenticationConstants.IdentityApplicationScheme;
                _.DefaultScheme = AuthenticationConstants.IdentityApplicationScheme;
                _.DefaultSignInScheme = AuthenticationConstants.IdentityApplicationScheme;
                _.DefaultSignOutScheme = AuthenticationConstants.IdentityApplicationScheme;
            })
            // Above we could use the following switching scheme and remove the
            // corresponding `app.UseWhen( ... app.UseAuthentication() )` in
            // `Startup.cs`. Inspired by
            // https://learn.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-10.0#use-multiple-authentication-schemes
            // .AddPolicyScheme("SwitchingScheme", "Identity, Cookie, or Bearer", options =>
            // {
            //     options.ForwardDefaultSelector = context =>
            //     {
            //         if (context.Request.Path.StartsWithSegments("/connect"))
            //         {
            //             return AuthenticationConstants.IdentityApplicationScheme;
            //         }
            //         return AuthenticationConstants.IdentityAndCookieAndBearerTokenAuthenticationScheme;
            //     };
            // })
            // The cookie is used by the metabase acting as its own client application through the
            // authentication scheme `CookieAuthenticationDefaults.AuthenticationScheme`, that is, "Cookies".
            .AddCookie(_ =>
            {
                _.AccessDeniedPath = "/unauthorized";
                _.LoginPath = "/connect/client/login";
                _.LogoutPath = "/connect/client/logout";
                _.ReturnUrlParameter = "returnTo";
                _.ExpireTimeSpan = s_cookieExpirationTimeSpan;
                _.SlidingExpiration = true;
            })
            .AddScheme<
                IdentityAndCookieAndBearerTokenAuthenticationSchemeOptions,
                IdentityAndCookieAndBearerTokenAuthenticationSchemeHandler
            >(
                AuthenticationConstants.IdentityAndCookieAndBearerTokenAuthenticationScheme,
                _ => { }
            );
        services.AddAuthorization(_ =>
            {
                _.AddPolicy(AuthorizationPolicies.AuthenticatedPolicy, policy =>
                    {
                        policy.AuthenticationSchemes =
                        [
                            AuthenticationConstants.IdentityAndCookieAndBearerTokenAuthenticationScheme
                        ];
                        policy.RequireAuthenticatedUser();
                    }
                );
                foreach (var (policyName, scope) in s_policyNameToOpenIdConnectScope)
                {
                    _.AddPolicy(policyName, policy =>
                        {
                            policy.AuthenticationSchemes =
                            [
                                AuthenticationConstants.IdentityAndCookieAndBearerTokenAuthenticationScheme
                            ];
                            policy.RequireAuthenticatedUser();
                            policy.RequireAssertion(context =>
                                {
                                    return context.User.HasScope(scope);
                                }
                            );
                        }
                    );
                }
            }
        );
    }

    private static void ConfigureTaskScheduling(
        IServiceCollection services,
        IWebHostEnvironment environment
    )
    {
        // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks (like
        // pruning orphaned authorizations/tokens from the database) at regular intervals. For
        // configuring Quartz see https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/hosted-services-integration.html
        services.AddQuartz(_ =>
        {
            _.SchedulerId = OpenIdConnectConstants.MetabaseQuartzSchedulerId;
            _.SchedulerName = "Metabase";
            if (environment.IsEnvironment(Program.TestEnvironment))
            {
                var probablyUniqueId = Guid.NewGuid().ToString();
                _.SchedulerId = $"{OpenIdConnectConstants.MetabaseQuartzSchedulerId}-{probablyUniqueId}";
                _.SchedulerName = $"Metabase-{probablyUniqueId}";
            }
            _.UseSimpleTypeLoader();
            _.UseInMemoryStore();
            _.UseDefaultThreadPool(_ =>
                _.MaxConcurrency = 10
            );
            var jwtSigningAndEncryptionKeyRotationJobKey = new JobKey(JwtSigningAndEncryptionCertificateRotationJob.KeyName);
            _.AddJob<JwtSigningAndEncryptionCertificateRotationJob>(_ => _.WithIdentity(jwtSigningAndEncryptionKeyRotationJobKey));
            _.AddTrigger(_ => _
                .ForJob(jwtSigningAndEncryptionKeyRotationJobKey)
                .WithIdentity(JwtSigningAndEncryptionCertificateRotationJob.TriggerIdentityName)
                .StartNow()
                .WithSimpleSchedule(_ => _
                    .WithIntervalInHours(24)
                    .RepeatForever()
                    .WithMisfireHandlingInstructionFireNow()
                )
            );
        });
        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        services.AddQuartzServer(_ =>
            _.WaitForJobsToComplete = true
        );
    }

    private static void ConfigureOpenIddictServices(
        IServiceCollection services,
        IWebHostEnvironment environment,
        AppSettings appSettings
    )
    {
        services.AddOpenIddict()
            // Register the OpenIddict core components.
            .AddCore(_ =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                _.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>()
                    .ReplaceDefaultEntities<OpenIdConnectApplication, Data.OpenIdConnect.OpenIdConnectAuthorization, OpenIdConnectScope, OpenIdConnectToken, Guid>();
                // Enable Quartz.NET integration.
                _.UseQuartz();
            })
            // Register the OpenIddict server components.
            .AddServer(_ =>
                {
                    _.SetIssuer(appSettings.Uri);
                    _
                        .SetAuthorizationEndpointUris("connect/authorize")
                        .SetConfigurationEndpointUris(".well-known/openid-configuration")
                        // .SetDeviceAuthorizationEndpointUris("connect/device")
                        .SetEndSessionEndpointUris("connect/endsession")
                        .SetEndUserVerificationEndpointUris("connect/verify")
                        .SetIntrospectionEndpointUris("connect/introspect")
                        .SetJsonWebKeySetEndpointUris(".well-known/jwks")
                        .SetPushedAuthorizationEndpointUris("connect/par")
                        .SetRevocationEndpointUris("connect/revocation")
                        .SetTokenEndpointUris("connect/token")
                        .SetUserInfoEndpointUris("connect/userinfo");
                    _.RegisterScopes([
                        OpenIddictConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.OpenId,
                        ..OpenIdConnectScope.Scopes
                    ]);
                    _
                        .AllowAuthorizationCodeFlow() // for user-to-machine communication
                        .AllowClientCredentialsFlow() // for machine-to-machine communication
                        .AllowRefreshTokenFlow() // for refreshing access tokens
                        .AllowTokenExchangeFlow(); // for issuing additional access tokens for one login/authorization
                    if (environment.IsEnvironment(Program.TestEnvironment))
                    {
                        _.AllowPasswordFlow();
                    }
                    // if (environment.IsEnvironment(Program.TestEnvironment))
                    // {
                    //     _.AddDevelopmentEncryptionCertificate();
                    //     _.AddDevelopmentSigningCertificate();
                    // }
                    // else
                    // {
                    // Register the signing and encryption credentials. See
                    // https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
                    // and https://stackoverflow.com/questions/50862755/signing-keys-certificates-and-client-secrets-confusion/50932120#50932120
                    foreach (var encryptionCertificate in FindCertificates(OpenIdConnectConstants.Server.EncryptionSubjectDistinguishedName))
                    {
                        _.AddEncryptionCertificate(encryptionCertificate);
                    }
                    foreach (var signingCertificate in FindCertificates(OpenIdConnectConstants.Server.SigningSubjectDistinguishedName))
                    {
                        _.AddSigningCertificate(signingCertificate);
                    }
                    // }
                    // Force client applications to use Proof Key for Code Exchange (PKCE): https://documentation.openiddict.com/configuration/proof-key-for-code-exchange.html#enabling-pkce-enforcement-at-the-global-level
                    _.RequireProofKeyForCodeExchange();
                    // Force client applications to use Pushed Authorization Requests (PAR): https://documentation.openiddict.com/configuration/pushed-authorization-requests
                    _.RequirePushedAuthorizationRequests();
                    // Default lifetimes can be seen in: https://github.com/openiddict/openiddict-core/blob/dev/src/OpenIddict.Server/OpenIddictServerOptions.cs
                    _
                        .SetAccessTokenLifetime(OpenIdConnectConstants.AccessAndIdentityTokenLifetime)
                        .SetIdentityTokenLifetime(OpenIdConnectConstants.AccessAndIdentityTokenLifetime)
                        .SetRefreshTokenLifetime(OpenIdConnectConstants.RefreshTokenLifetime);
                    // https://documentation.openiddict.com/integrations/aspnet-core#authorization-and-logout-request-caching
                    _
                        .EnableAuthorizationRequestCaching()
                        .EnableEndSessionRequestCaching();
                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    var builder = _
                        .UseAspNetCore()
                        .SuppressJsonResponseIndentation()
                        .EnableAuthorizationEndpointPassthrough() // https://documentation.openiddict.com/integrations/aspnet-core#pass-through-mode
                        .EnableEndSessionEndpointPassthrough()
                        .EnableEndUserVerificationEndpointPassthrough()
                        .EnableStatusCodePagesIntegration() // https://documentation.openiddict.com/integrations/aspnet-core#status-code-pages-middleware-integration
                        .EnableTokenEndpointPassthrough()
                        .EnableUserInfoEndpointPassthrough();
                    if (environment.IsEnvironment(Program.TestEnvironment))
                    {
                        builder.DisableTransportSecurityRequirement(); // https://documentation.openiddict.com/integrations/aspnet-core#transport-security-requirement
                    }
                    _.RegisterAudiences(OpenIdConnectConstants.Client.MetabaseClientId);
                    _.RegisterResources(appSettings.GraphQlEndpoint);
                    // Disable and ignore audiences
                    // https://documentation.openiddict.com/guides/migration/60-to-70#register-audiences-and-resources-if-applicable
                    // _
                    //     .DisableAudienceValidation()
                    //     .DisableResourceValidation();
                    // _
                    //     .IgnoreAudiencePermissions()
                    //     .IgnoreResourcePermissions();
                    // _.UseDataProtection();
                    // Note: if you don't want to specify a client_id when sending a token or
                    // revocation request, uncomment the following line: _.AcceptAnonymousClients();
                    // Note: if you want to process authorization and token requests that specify
                    // non-registered scopes, uncomment the following line: _.DisableScopeValidation();
                    // Note: if you don't want to use permissions, you can disable permission
                    // enforcement by uncommenting the following lines:
                    // _.IgnoreEndpointPermissions() .IgnoreGrantTypePermissions()
                    // .IgnoreResponseTypePermissions() .IgnoreScopePermissions();
                    // Note: when issuing access tokens used by third-party APIs you don't own, you
                    // can disable access token encryption: _.DisableAccessTokenEncryption();
                    // Note: To decrypt a JWT token, like an application code or an access token,
                    // you can execute ``` var jwt =
                    // "eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwidHlwIjoiYXQrand0In0.tn0jqCR5-01c_SDRwex6sPBNl1Vl1KSRA5Zs2UoXri9F4tG4v9B6qbLmxzsfkd0rLe55BozRV8VCChsZAt_mCZfoVGOYewwP.ogGZQ5Q2p7Yue7D6zPJlSQ.uNX1Qs9R30vZxAPj-LuJSwWnisoKHQ5qNC_K9WvA0JDSCS_orw6TsboSVCe5b_aUg3JvrkJX_Ir0c-bFMf0HVhOVNy1UJwxo9zegJOsm7MybPzK58H4ubt_PRnrSueLgnQX8aDcjbM38Imy3RN6a3r3aKawdWGcyA23sIH8XnVmGk0lDB_PqFrFE7x2MmG4fyVJINoki441UI-7x0sLFUi4o98Z-2vTFuCd9cLRY5LAeb0ZIuWwOI7dv2Q54w7uV765kHS3VIPtupzSSXgQmfPBJOzDeV_-sCZGwUuC0jL8x1vJw573fejPwpPmXj3EKgzXgbGfAHmjoIzkNvcfr--Dy7O8WxLjSERoBgW45Tq0xcCLL9Vx4JGWehOw1jY-KaKIzfjUW9CTSwLgWdhqonetAULZFRJAYOEJ8PtP49jXlvkdjmavRXaX2UD8FD5TUx30TkGQ_xyuo6HvokURQqIYILZiL6R_kVqh7kwUnj4dgBqni-56M1GYFuX2UzynagL6c6t4OlO7RhQJpgz1pzmGNdEb5nvZplXru8KVEt_e9bQOy54EWudAemEVAyX-4P6FdWWazU5vMaRnDg53Y8gy4rt6LCwXK9WHNRbhkdAmTNNsfiuUd588lzMJVSVPeqzbl70yW8IH7hbwfscLjSf2-gP9evwDKoLMKjqyBtbsqGx-qtBE47y_-LKyP3TOJug0SBtiHIMb3xzN2rJaykAMRRz4kMaMF4_TZRV8kqZQqzF_xBoMJQD3nLYaN-G0qJYIse1JhVu4yHuH7vSvXdHA0x5dnBwwq4P3g35W5zv-cw4-b1cXnLq7TYUvHNVe6DcMAJogxW5ovA7wtEjF4yKxSbvlRaO065Jed5siLrIL17RahoHqew34kMzqXL0OUHvxId0A7myvLFy0YqhLnKg.1jfT6-IVamiQbS84hfX4lLtmoTiAmf0Ea0rpLPgYEek";
                    // var handler = new JwtSecurityTokenHandler(); var claimsPrincipal =
                    // handler.ValidateToken( jwt, new TokenValidationParameters { IssuerSigningKey
                    // = signingKey, TokenDecryptionKey = encryptionKey, // ValidIssuer =
                    // environment.IsEnvironment(Program.TestEnvironment) ? "http://localhost/" :
                    // appSettings.Host, ValidateActor = false, ValidateAudience = false,
                    // ValidateIssuer = false, ValidateIssuerSigningKey = false, ValidateLifetime =
                    // false, ValidateTokenReplay = false, }, out var validatedToken );
                    // Console.WriteLine(validatedToken.ToString()); ``` which as of this writing
                    // outputs ```
                    // {"alg":"A256KW","enc":"A256CBC-HS512","typ":"at+jwt"}.{"sub":"075561fa-98c0-40db-ad3d-9dc8abf240fd","name":"sw@ise.de","http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":"sw@ise.de","oi_prst":"metabase","iss":"https://localhost:4041/","oi_au_id":"5dc00347-30fa-4ddb-91c1-471c505e7842","client_id":"metabase","oi_tkn_id":"e7430a26-4e13-4c21-8e6d-0f07dca66ff6","aud":"metabase","scope":"openid
                    // email profile roles api:read api:write
                    // offline_access","exp":1615405869,"iat":1615402269} ```
                }
            )
            // Register the OpenIddict validation components.
            .AddValidation(_ =>
            {
                _.SetIssuer(appSettings.Uri);
                // Configure the audience accepted by this resource server.
                _.AddAudiences(OpenIdConnectConstants.Client.MetabaseClientId);
                // Import the configuration from the local OpenIddict server instance:
                // https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#using-the-optionsuselocalserver-integration
                // Alternatively, OpenId Connect discovery can be used: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#using-openid-connect-discovery-asymmetric-signing-keys-only
                _.UseLocalServer();
                // Register the ASP.NET Core host.
                _.UseAspNetCore();
                // Enable token entry validation: https://documentation.openiddict.com/configuration/token-storage.html#enabling-token-entry-validation-at-the-api-level
                _.EnableTokenEntryValidation();
                // Enable authorization entry validation: https://documentation.openiddict.com/configuration/authorization-storage.html#enabling-authorization-entry-validation-at-the-api-level
                _.EnableAuthorizationEntryValidation();
                // Register the System.Net.Http integration.
                _.UseSystemNetHttp()
                    .ConfigureHttpClientHandler(handler =>
                    {
                        if (environment.IsDevelopment())
                        {
                            // https://documentation.openiddict.com/integrations/system-net-http#register-a-custom-httpclienthandler-configuration-delegate
                            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                        }
                    });
            })
            .AddClient(_ =>
            {
                _.AllowAuthorizationCodeFlow()
                 .AllowClientCredentialsFlow()
                 .AllowRefreshTokenFlow()
                 .AllowTokenExchangeFlow();

                // Register the signing and encryption credentials. See https://stackoverflow.com/questions/50862755/signing-keys-certificates-and-client-secrets-confusion/50932120#50932120
                foreach (var encryptionCertificate in FindCertificates(OpenIdConnectConstants.Client.EncryptionSubjectDistinguishedName))
                {
                    _.AddEncryptionCertificate(encryptionCertificate);
                }
                foreach (var signingCertificate in FindCertificates(OpenIdConnectConstants.Client.SigningSubjectDistinguishedName))
                {
                    _.AddSigningCertificate(signingCertificate);
                }

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                _.UseAspNetCore()
                 .EnableStatusCodePagesIntegration() // https://documentation.openiddict.com/integrations/aspnet-core#status-code-pages-middleware-integration
                 .EnableRedirectionEndpointPassthrough() // https://documentation.openiddict.com/integrations/aspnet-core#pass-through-mode
                 .EnablePostLogoutRedirectionEndpointPassthrough();
                // .DisableTransportSecurityRequirement(); // https://documentation.openiddict.com/integrations/aspnet-core#transport-security-requirement

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                _.UseSystemNetHttp()
                    .SetProductInformation(typeof(Startup).Assembly)
                    .ConfigureHttpClientHandler(handler =>
                    {
                        if (environment.IsDevelopment())
                        {
                            // https://documentation.openiddict.com/integrations/system-net-http#register-a-custom-httpclienthandler-configuration-delegate
                            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                        }
                    });

                // Add a client registration matching the client application definition in the
                // server project.
                var clientRegistration = new OpenIddictClientRegistration
                {
                    RegistrationId = OpenIdConnectConstants.Client.MetabaseRegistrationId,
                    Issuer = appSettings.Uri,

                    // Note: these settings must match the application details inserted in the
                    // database at the server level.
                    ClientId = OpenIdConnectConstants.Client.MetabaseClientId,
                    ClientSecret = appSettings.OpenIdConnectClientSecret,

                    // Note: to mitigate mix-up attacks, it's recommended to use a unique
                    // redirection endpoint URI per provider, unless all the registered
                    // providers support returning a special "iss" parameter containing their
                    // URL as part of authorization responses. For more information, see https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4.
                    RedirectUri = new Uri($"connect/callback/login/{OpenIdConnectConstants.Client.MetabaseClientId}", UriKind.Relative),
                    PostLogoutRedirectUri = new Uri($"connect/callback/logout/{OpenIdConnectConstants.Client.MetabaseClientId}", UriKind.Relative)
                };
                clientRegistration.Scopes.UnionWith([
                    OpenIddictConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.OpenId,
                    ..OpenIdConnectScope.Scopes
                ]);
                _.AddRegistration(clientRegistration);
            });
    }
}