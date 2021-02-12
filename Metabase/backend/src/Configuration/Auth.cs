using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using Quartz;
using AppSettings = Infrastructure.AppSettings;

namespace Metabase.Configuration
{
    public abstract class Auth
    {
        public static string ApiScope { get; } = "api";
        public static string ServerName { get; } = "metabase";

        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            AppSettings appSettings
            )
        {
            // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            var encryptionKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    appSettings.JsonWebToken.EncryptionKey
                    )
                );
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    appSettings.JsonWebToken.SigningKey
                    )
                );
            ConfigureIdentityServices(services);
            ConfigureAuthenticiationAndAuthorizationServices(services, environment, appSettings, encryptionKey, signingKey);
            ConfigureTaskScheduling(services, environment);
            ConfigureOpenIddictServices(services, environment, encryptionKey, signingKey);
        }

        private static void ConfigureIdentityServices(
            IServiceCollection services
            )
        {
            services.AddIdentity<Data.User, Data.Role>(_ =>
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
                    _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    _.Lockout.MaxFailedAccessAttempts = 5;
                    _.Lockout.AllowedForNewUsers = true;
                    // User settings.
                    _.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    _.User.RequireUniqueEmail = true;
                    // Configure Identity to use the same JWT claims as
                    // OpenIddict instead of the legacy WS-Federation claims it
                    // uses by default (ClaimTypes), which saves you from doing
                    // the mapping in your authorization controller.
                    _.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                    _.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                    _.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                })
              .AddEntityFrameworkStores<Data.ApplicationDbContext>()
              .AddDefaultTokenProviders();
        }

        private static void ConfigureAuthenticiationAndAuthorizationServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            AppSettings appSettings,
            SymmetricSecurityKey encryptionKey,
            SymmetricSecurityKey signingKey
            )
        {
            // https://openiddict.github.io/openiddict-documentation/configuration/token-setup-and-validation.html#jwt-validation
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/
            services.AddAuthentication(_ =>
             {
                 _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 _.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
             }
             )
              .AddJwtBearer(_ =>
                  {
                      _.Audience = ServerName;
                      _.RequireHttpsMetadata = false; // TODO `!environment.IsEnvironment("test");` ... but what about server-side requests from next.js?
                      _.IncludeErrorDetails = true;
                      _.SaveToken = true;
                      _.TokenValidationParameters = new TokenValidationParameters()
                      {
                          NameClaimType = OpenIddictConstants.Claims.Subject,
                          RoleClaimType = OpenIddictConstants.Claims.Role,
                          ValidateIssuer = true,
                          ValidIssuer = environment.IsEnvironment("test") ? "http://localhost/" : appSettings.Host,
                          RequireAudience = true,
                          ValidateAudience = true,
                          ValidAudience = ServerName,
                          RequireExpirationTime = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          RequireSignedTokens = true,
                          TokenDecryptionKey = encryptionKey,
                          IssuerSigningKey = signingKey
                      };
                  });
            services.AddAuthorization();
        }

        private static void ConfigureTaskScheduling(
          IServiceCollection services,
            IWebHostEnvironment environment
        )
        {
            // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
            // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
            services.AddQuartz(_ =>
            {
                // TODO Configure properly. See https://github.com/quartznet/quartznet/blob/master/src/Quartz.Extensions.DependencyInjection/IServiceCollectionQuartzConfigurator.cs
                _.UseMicrosoftDependencyInjectionJobFactory();
                _.UseSimpleTypeLoader();
                _.UseInMemoryStore(); // TODO `UsePersistentStore`?
                if (environment.IsEnvironment("test"))
                {
                    // See https://gitter.im/MassTransit/MassTransit?at=5db2d058f6db7f4f856fb404
                    _.SchedulerName = Guid.NewGuid().ToString();
                }
            });
            // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
            services.AddQuartzHostedService(options =>
                options.WaitForJobsToComplete = true
                );
        }

        private static void ConfigureOpenIddictServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            SymmetricSecurityKey encryptionKey,
            SymmetricSecurityKey signingKey
            )
        {
            services.AddOpenIddict()
              // Register the OpenIddict core components.
              .AddCore(_ =>
                  {
                      // Configure OpenIddict to use the Entity Framework Core stores and models.
                      // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                      _.UseEntityFrameworkCore()
                      .UseDbContext<Data.ApplicationDbContext>();
                      // Enable Quartz.NET integration.
                      _.UseQuartz();
                  }
                  )
            // Register the OpenIddict server components.
            .AddServer(_ =>
                {
                    _.SetAuthorizationEndpointUris("/connect/authorize")
                               .SetDeviceEndpointUris("/connect/device")
                               .SetLogoutEndpointUris("/connect/logout")
                               .SetIntrospectionEndpointUris("/connect/introspect")
                               .SetTokenEndpointUris("/connect/token")
                               .SetUserinfoEndpointUris("/connect/userinfo")
                               .SetVerificationEndpointUris("/connect/verify");
                    _.RegisterScopes(
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Roles,
                        ApiScope
                        );
                    _.AllowAuthorizationCodeFlow()
                      .AllowDeviceCodeFlow()
                      .AllowRefreshTokenFlow();
                    // .AllowHybridFlow()
                    if (environment.IsEnvironment("test"))
                    {
                        _.AllowPasswordFlow();
                    }
                    // Register the signing and encryption credentials.
                    // TODO Use certificate (public-private key pair) instead of symmetric keys. OpenIddict requires at least one certificate anyway.
                    _.AddEncryptionKey(encryptionKey)
                    .AddSigningKey(signingKey);
                    if (environment.IsDevelopment())
                    {
                        _.AddDevelopmentEncryptionCertificate()
                         .AddDevelopmentSigningCertificate();
                        // _.AddEphemeralEncryptionKey()
                        // .AddEphemeralSigningKey();
                    }
                    else if (environment.IsEnvironment("test"))
                    {
                        _.AddDevelopmentEncryptionCertificate(new X500DistinguishedName($"CN=OpenIddict Server Encryption Certificate {Guid.NewGuid()}"))
                         .AddDevelopmentSigningCertificate(new X500DistinguishedName($"CN=OpenIddict Server Signing Certificate {Guid.NewGuid()}"));
                    }
                    else
                    {
                        // JWTs must be signed by a self-signing certificate or
                        // a symmetric key. Here a certificate is used. The
                        // certificate is an embedded resource, see the csproj
                        // file. The certificate must contain public and private
                        // keys.
                        // TODO Manage the certificate and its password properly in production. Also use the same approach in development to match the production environment as closely as possible.
                        _.AddEncryptionCertificate(
                        assembly: typeof(Startup).GetTypeInfo().Assembly,
                        resource: "Metabase.jwt-encryption-certificate.pfx",
                        password: "password"
                        )
                        .AddSigningCertificate(
                        assembly: typeof(Startup).GetTypeInfo().Assembly,
                        resource: "Metabase.jwt-signing-certificate.pfx",
                        password: "password"
                        );
                    }
                    // Force client applications to use Proof Key for Code Exchange (PKCE).
                    _.RequireProofKeyForCodeExchange();
                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    var aspNetCoreBuilder =
                        _.UseAspNetCore()
                               .EnableStatusCodePagesIntegration()
                               .EnableAuthorizationEndpointPassthrough()
                               .EnableLogoutEndpointPassthrough()
                               .EnableTokenEndpointPassthrough()
                               .EnableUserinfoEndpointPassthrough()
                               .EnableVerificationEndpointPassthrough();
                    if (true) // TODO `environment.IsEnvironment("test")` but what about server-side requests from next.js?
                    {
                        aspNetCoreBuilder.DisableTransportSecurityRequirement();
                    }
                    // Note: if you don't want to specify a client_id when sending
                    // a token or revocation request, uncomment the following line:
                    // _.AcceptAnonymousClients();
                    // Note: if you want to process authorization and token requests
                    // that specify non-registered scopes, uncomment the following line:
                    // _.DisableScopeValidation();
                    // Note: if you don't want to use permissions, you can disable
                    // permission enforcement by uncommenting the following lines:
                    // _.IgnoreEndpointPermissions()
                    //        .IgnoreGrantTypePermissions()
                    //        .IgnoreResponseTypePermissions()
                    //        .IgnoreScopePermissions();
                    // Note: when issuing access tokens used by third-party APIs
                    // you don't own, you can disable access token encryption:
                    // _.DisableAccessTokenEncryption();
                }
            )
              // Register the OpenIddict validation components.
              .AddValidation(_ =>
                 {
                     // Configure the audience accepted by this resource server.
                     _.AddAudiences(ServerName);
                     // Import the configuration from the local OpenIddict server instance.
                     _.UseLocalServer();
                     // Register the ASP.NET Core host.
                     _.UseAspNetCore();
                     // Note: the validation handler uses OpenID Connect discovery
                     // to retrieve the address of the introspection endpoint.
                     //options.SetIssuer("http://localhost:12345/");
                     // Configure the validation handler to use introspection and register the client
                     // credentials used when communicating with the remote introspection endpoint.
                     //options.UseIntrospection()
                     //       .SetClientId("resource_server_1")
                     //       .SetClientSecret("846B62D0-DEF9-4215-A99D-86E6B8DAB342");
                     // Register the System.Net.Http integration.
                     //options.UseSystemNetHttp();
                 });
        }
    }
}