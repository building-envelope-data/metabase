using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using System.Reflection;
using System.IO;

namespace Metabase.Configuration
{
    public abstract class AuthConfiguration
    {
        // public const string JwtBearerAuthenticatedPolicy = "JwtBearerAuthenticated";
        public const string ReadPolicy = "Read";
        public const string WritePolicy = "Write";
        public const string ManageUserPolicy = "ManageUser";
        public static string ReadApiScope { get; } = "api:read";
        public static string WriteApiScope { get; } = "api:write";
        public static string ManageUserApiScope { get; } = "api:user:manage";

        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            AppSettings appSettings
            )
        {
            var encryptionCertificate = LoadCertificate("jwt-encryption-certificate.pfx", appSettings.JsonWebToken.EncryptionCertificatePassword);
            var signingCertificate = LoadCertificate("jwt-signing-certificate.pfx", appSettings.JsonWebToken.SigningCertificatePassword);
            ConfigureIdentityServices(services);
            ConfigureAuthenticiationAndAuthorizationServices(services, environment, appSettings, encryptionCertificate, signingCertificate);
            ConfigureTaskScheduling(services, environment);
            ConfigureOpenIddictServices(services, environment, appSettings, encryptionCertificate, signingCertificate);
        }

        private static X509Certificate2 LoadCertificate(
            string fileName,
            string password
        )
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception($"Empty password for certificate {fileName}.");
            }
            var stream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream($"Metabase.{fileName}")
                ?? throw new Exception($"Missing certificate {fileName}.");
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            return new X509Certificate2(
                buffer.ToArray(),
                password,
                X509KeyStorageFlags.EphemeralKeySet
            );
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
            services.ConfigureApplicationCookie(_ =>
            {
                _.AccessDeniedPath = "/unauthorized";
                _.LoginPath = "/users/login";
                _.LogoutPath = "/me/logout";
                _.ReturnUrlParameter = "returnTo";
                _.Events.OnValidatePrincipal = context =>
                {
                    // The metabase frontend uses application cookies for
                    // user authentication and is allowed to read data,
                    // write data, and manage users. The corresponding
                    // policies use scopes, so we need to add them.
                    if (context?.Principal is not null)
                    {
                        var identity = new ClaimsIdentity();
                        foreach (
                            var claim in
                                new[] {
                                    ReadApiScope,
                                    WriteApiScope,
                                    ManageUserApiScope
                                }
                        )
                        {
                            identity.AddClaim(
                                new Claim(
                                    OpenIddictConstants.Claims.Private.Scope,
                                    claim
                                    )
                                );
                        }
                        context.Principal.AddIdentity(identity);
                    }
                    return Task.CompletedTask;
                };
            }
            );
        }

        private static void ConfigureAuthenticiationAndAuthorizationServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            AppSettings appSettings,
            X509Certificate2 encryptionCertificate,
            X509Certificate2 signingCertificate
            )
        {
            // https://openiddict.github.io/openiddict-documentation/configuration/token-setup-and-validation.html#jwt-validation
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/
            services.AddAuthentication()
              .AddJwtBearer(_ =>
                  {
                      _.Audience = appSettings.Host;
                      _.RequireHttpsMetadata = !environment.IsEnvironment("test");
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
                          ValidAudience = appSettings.Host,
                          RequireExpirationTime = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          RequireSignedTokens = true,
                          TokenDecryptionKey = new X509SecurityKey(encryptionCertificate),
                          IssuerSigningKey = new X509SecurityKey(signingCertificate)
                      };
                      // _.Events.OnAuthenticationFailed = _ =>
                  });
            services.AddAuthorization(_ =>
            {
                // _.AddPolicy(JwtBearerAuthenticatedPolicy, policy =>
                // {
                //     policy.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme }; // For cookies it would be `IdentityConstants.ApplicationScheme`
                //     policy.RequireAuthenticatedUser();
                // }
                // );
                foreach (var (policyName, scope) in new[] {
                     (ReadPolicy, ReadApiScope),
                     (WritePolicy, WriteApiScope),
                     (ManageUserPolicy, ManageUserApiScope)
                     }
                   )
                {
                    _.AddPolicy(policyName, policy =>
                    {
                        policy.AuthenticationSchemes = new[] {
                            OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme
                            };
                        policy.RequireAuthenticatedUser();
                        policy.RequireAssertion(context =>
                        {
                            // How the `HttpContext` can be accessed when the policies are used in GraphQL queries or mutations.
                            // if (context.Resource is IResolverContext resolverContext)
                            // {
                            //     if (resolverContext.ContextData.ContainsKey(nameof(HttpContext)))
                            //     {
                            //         if (resolverContext.ContextData[nameof(HttpContext)] is HttpContext httpContext)
                            //         {
                            //             if (httpContext.Request.Headers.ContainsKey("Sec-Fetch-Site") &&
                            //                 httpContext.Request.Headers.ContainsKey("Origin")
                            //                 )
                            //             {
                            //                 // Note that CORS cannot serve as a security mechanism. Secure access from the frontend by some other means.
                            //                 return httpContext.Request.Headers["Sec-Fetch-Site"] == "same-origin" &&
                            //                     httpContext.Request.Host == httpContext.Request.Headers["Origin"]; // Comparison does not work because one includes the protocol HTTPS while the other does not.
                            //             }
                            //         }

                            //     }
                            // }
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
            // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
            // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
            // For configuring Quartz see
            // https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/hosted-services-integration.html
            services.AddQuartz(_ =>
            {
                _.SchedulerId = "metabase";
                _.SchedulerName = "Metabase";
                _.UseMicrosoftDependencyInjectionJobFactory();
                _.UseSimpleTypeLoader();
                _.UseInMemoryStore();
                _.UseDefaultThreadPool(_ =>
                    _.MaxConcurrency = 10
                );
                if (environment.IsEnvironment("test"))
                {
                    // See https://gitter.im/MassTransit/MassTransit?at=5db2d058f6db7f4f856fb404
                    _.SchedulerName = Guid.NewGuid().ToString();
                }
            });
            // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
            services.AddQuartzHostedService(_ =>
                _.WaitForJobsToComplete = true
                );
        }

        private static void ConfigureOpenIddictServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            AppSettings appSettings,
            X509Certificate2 encryptionCertificate,
            X509Certificate2 signingCertificate
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
                    _.SetAuthorizationEndpointUris("connect/authorize")
                               .SetDeviceEndpointUris("connect/device")
                               .SetLogoutEndpointUris("connect/logout")
                               .SetIntrospectionEndpointUris("connect/introspect")
                               // .SetRevocationEndpointUris("")
                               // .SetCryptographyEndpointUris("")
                               // .SetConfigurationEndpointUris("")
                               .SetTokenEndpointUris("connect/token")
                               .SetUserinfoEndpointUris("connect/userinfo")
                               .SetVerificationEndpointUris("connect/verify");
                    _.RegisterScopes(
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Roles,
                        ReadApiScope,
                        WriteApiScope,
                        ManageUserApiScope
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
                    // See https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios
                    // and https://stackoverflow.com/questions/50862755/signing-keys-certificates-and-client-secrets-confusion/50932120#50932120
                    _.AddEncryptionCertificate(encryptionCertificate)
                    .AddSigningCertificate(signingCertificate);
                    // Force client applications to use Proof Key for Code Exchange (PKCE): https://documentation.openiddict.com/configuration/proof-key-for-code-exchange.html#enabling-pkce-enforcement-at-the-global-level
                    _.RequireProofKeyForCodeExchange();
                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    _.UseAspNetCore()
                     .EnableStatusCodePagesIntegration()
                     .EnableAuthorizationEndpointPassthrough()
                     .EnableLogoutEndpointPassthrough()
                     .EnableTokenEndpointPassthrough()
                     .EnableUserinfoEndpointPassthrough()
                     .EnableVerificationEndpointPassthrough();
                    // .EnableStatusCodePagesIntegration();
                    // .DisableTransportSecurityRequirement();
                    // _.UseDataProtection();
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
                    // Note: To decrypt a JWT token, like an application code or an access token, you can execute
                    // ```
                    // var jwt = "eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwidHlwIjoiYXQrand0In0.tn0jqCR5-01c_SDRwex6sPBNl1Vl1KSRA5Zs2UoXri9F4tG4v9B6qbLmxzsfkd0rLe55BozRV8VCChsZAt_mCZfoVGOYewwP.ogGZQ5Q2p7Yue7D6zPJlSQ.uNX1Qs9R30vZxAPj-LuJSwWnisoKHQ5qNC_K9WvA0JDSCS_orw6TsboSVCe5b_aUg3JvrkJX_Ir0c-bFMf0HVhOVNy1UJwxo9zegJOsm7MybPzK58H4ubt_PRnrSueLgnQX8aDcjbM38Imy3RN6a3r3aKawdWGcyA23sIH8XnVmGk0lDB_PqFrFE7x2MmG4fyVJINoki441UI-7x0sLFUi4o98Z-2vTFuCd9cLRY5LAeb0ZIuWwOI7dv2Q54w7uV765kHS3VIPtupzSSXgQmfPBJOzDeV_-sCZGwUuC0jL8x1vJw573fejPwpPmXj3EKgzXgbGfAHmjoIzkNvcfr--Dy7O8WxLjSERoBgW45Tq0xcCLL9Vx4JGWehOw1jY-KaKIzfjUW9CTSwLgWdhqonetAULZFRJAYOEJ8PtP49jXlvkdjmavRXaX2UD8FD5TUx30TkGQ_xyuo6HvokURQqIYILZiL6R_kVqh7kwUnj4dgBqni-56M1GYFuX2UzynagL6c6t4OlO7RhQJpgz1pzmGNdEb5nvZplXru8KVEt_e9bQOy54EWudAemEVAyX-4P6FdWWazU5vMaRnDg53Y8gy4rt6LCwXK9WHNRbhkdAmTNNsfiuUd588lzMJVSVPeqzbl70yW8IH7hbwfscLjSf2-gP9evwDKoLMKjqyBtbsqGx-qtBE47y_-LKyP3TOJug0SBtiHIMb3xzN2rJaykAMRRz4kMaMF4_TZRV8kqZQqzF_xBoMJQD3nLYaN-G0qJYIse1JhVu4yHuH7vSvXdHA0x5dnBwwq4P3g35W5zv-cw4-b1cXnLq7TYUvHNVe6DcMAJogxW5ovA7wtEjF4yKxSbvlRaO065Jed5siLrIL17RahoHqew34kMzqXL0OUHvxId0A7myvLFy0YqhLnKg.1jfT6-IVamiQbS84hfX4lLtmoTiAmf0Ea0rpLPgYEek";
                    // var handler = new JwtSecurityTokenHandler();
                    // var claimsPrincipal = handler.ValidateToken(
                    //     jwt,
                    //     new TokenValidationParameters
                    //     {
                    //         IssuerSigningKey = signingKey,
                    //         TokenDecryptionKey = encryptionKey,
                    //         // ValidIssuer = environment.IsEnvironment("test") ? "http://localhost/" : appSettings.Host,
                    //         ValidateActor = false,
                    //         ValidateAudience = false,
                    //         ValidateIssuer = false,
                    //         ValidateIssuerSigningKey = false,
                    //         ValidateLifetime = false,
                    //         ValidateTokenReplay = false,
                    //     },
                    //     out var validatedToken
                    // );
                    // Console.WriteLine(validatedToken.ToString());
                    // ```
                    // which as of this writing outputs
                    // ```
                    // {"alg":"A256KW","enc":"A256CBC-HS512","typ":"at+jwt"}.{"sub":"075561fa-98c0-40db-ad3d-9dc8abf240fd","name":"sw@ise.de","http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":"sw@ise.de","oi_prst":"metabase","iss":"https://localhost:4041/","oi_au_id":"5dc00347-30fa-4ddb-91c1-471c505e7842","client_id":"metabase","oi_tkn_id":"e7430a26-4e13-4c21-8e6d-0f07dca66ff6","aud":"metabase","scope":"openid email profile roles api:read api:write offline_access","exp":1615405869,"iat":1615402269}
                    // ```
                }
            )
              // Register the OpenIddict validation components.
              .AddValidation(_ =>
                 {
                     // Configure the audience accepted by this resource server.
                     _.AddAudiences(appSettings.Host);
                     // Import the configuration from the local OpenIddict server instance: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#using-the-optionsuselocalserver-integration
                     // Alternatively, OpenId Connect discovery can be used: https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#using-openid-connect-discovery-asymmetric-signing-keys-only
                     _.UseLocalServer();
                     // Register the ASP.NET Core host.
                     _.UseAspNetCore();
                     // Enable token entry validation: https://documentation.openiddict.com/configuration/token-storage.html#enabling-token-entry-validation-at-the-api-level
                     _.EnableTokenEntryValidation();
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