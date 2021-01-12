using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenIddict.Abstractions;
using System.Reflection;
using Quartz;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace Metabase.Configuration
{
    public abstract class Auth
    {
        public static string ApiScope { get; } = "api";

        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            string assemblyName
            )
        {
            // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            ConfigureIdentityServices(services);
            ConfigureAuthenticiationAndAuthorizationServices(services, environment, configuration);
            ConfigureTaskScheduling(services, environment);
            ConfigureOpenIddictServices(services, environment, assemblyName);
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
                    _.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
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
            IConfiguration configuration
            )
        {
            // https://openiddict.github.io/openiddict-documentation/configuration/token-setup-and-validation.html#jwt-validation
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(_ =>
                  {
                      _.Authority = environment.IsEnvironment("test") ? "http://localhost" : "http://localhost:8080"; // TODO Make this configurable via environment variables through `configuration` (maybe its `AppSettings`).
                      _.Audience = "metabase"; // TODO This must be included in ticket creation. What does this mean?
                      _.RequireHttpsMetadata = false;
                      _.IncludeErrorDetails = true; //
                      _.TokenValidationParameters = new TokenValidationParameters()
                      {
                          NameClaimType = OpenIddictConstants.Claims.Subject,
                          RoleClaimType = OpenIddictConstants.Claims.Role,
                          // TODO Configure the following as in https://github.com/openiddict/openiddict-samples/issues/63#issuecomment-396785919 ?
                          // ValidateIssuer = true,
                          // ValidateIssuerSigningKey = true,
                          // ValidAudiences = new List<string> { "metabase" },
                          // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("953183eb-988d-403e-8b8a-7dd9cd44e303")) // using same GUID secret as above
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
            string assemblyName
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
                    // Enable the authorization, logout, token and userinfo endpoints.
                    _.SetAuthorizationEndpointUris("/connect/authorize")
                               .SetDeviceEndpointUris("/connect/device")
                               .SetLogoutEndpointUris("/connect/logout")
                               .SetIntrospectionEndpointUris("/connect/introspect")
                               .SetTokenEndpointUris("/connect/token")
                               .SetUserinfoEndpointUris("/connect/userinfo")
                               .SetVerificationEndpointUris("/connect/verify");
                    // Mark the "email", "profile" and "roles" scopes as supported scopes.
                    _.RegisterScopes(
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Roles,
                        ApiScope
                        );
                    // Allow flows.
                    _.AllowAuthorizationCodeFlow()
                      .AllowDeviceCodeFlow()
                      .AllowRefreshTokenFlow();
                    // .AllowHybridFlow()
                    if (environment.IsEnvironment("test"))
                    {
                        _.AllowPasswordFlow();
                    }
                    // Register the signing and encryption credentials.
                    if (environment.IsDevelopment())
                    {
                        _.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
                        // _.AddEphemeralSigningKey()
                    }
                    else if (environment.IsEnvironment("test"))
                    {
                        _.AddDevelopmentEncryptionCertificate(new X500DistinguishedName($"CN=OpenIddict Server Encryption Certificate {Guid.NewGuid()}"))
                        .AddDevelopmentSigningCertificate(new X500DistinguishedName($"CN=OpenIddict Server Signing Certificate {Guid.NewGuid()}"));
                        // _.AddSigningKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())))
                    }
                    else
                    {
                        // https://openiddict.github.io/openiddict-documentation/configuration/token-setup-and-validation.html#jwt-generation
                        /* _.UseJsonWebTokens(); */
                        // JWTs must be signed by a self-signing certificate or
                        // a symmetric key Here a certificate is used. The
                        // certificate is an embedded resource, see the csproj
                        // file. The certificate must contain public and private
                        // keys.
                        // TODO Manage the certificate and its password properly in production. Also use the same approach in development to match the production environment as closely as possible.
                        _.AddEncryptionCertificate(
                        assembly: typeof(Startup).GetTypeInfo().Assembly, // TODO ? use `assemblyName`?
                        resource: "Metabase.jwt-encryption-certificate.pfx",
                        password: "password"
                        )
                        .AddSigningCertificate(
                        assembly: typeof(Startup).GetTypeInfo().Assembly, // TODO ? use `assemblyName`?
                        resource: "Metabase.jwt-signing-certificate.pfx",
                        password: "password"
                        );
                        // TODO Or shall we use symmetric keys? (of course without hard-coding it here)
                        // options.AddSigningKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("953183eb-988d-403e-8b8a-7dd9cd44e303")));
                    }
                    // Force client applications to use Proof Key for Code Exchange (PKCE).
                    _.RequireProofKeyForCodeExchange();
                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    _.UseAspNetCore()
                               .EnableStatusCodePagesIntegration()
                               .EnableAuthorizationEndpointPassthrough()
                               .EnableLogoutEndpointPassthrough()
                               .EnableTokenEndpointPassthrough()
                               .EnableUserinfoEndpointPassthrough()
                               .EnableVerificationEndpointPassthrough()
                               .DisableTransportSecurityRequirement();
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
                      _.AddAudiences("metabase");
                      // Import the configuration from the local OpenIddict server instance.
                      _.UseLocalServer();
                      // Register the ASP.NET Core host.
                      _.UseAspNetCore();
                      // TODO ?
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
            // Register the worker responsible of seeding the database with the sample clients.
            // Note: in a real world application, this step should be part of a setup script.
            /* TODO services.AddHostedService<Worker>(); */
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}