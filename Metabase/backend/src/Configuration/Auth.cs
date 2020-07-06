using System;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Validation;
using Metabase.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Metabase.Configuration
{
    public sealed class Auth
    {
        public static readonly string ApiName = "api";
        public static readonly string ApiSecret = "secret"; // TODO Put in environment variable.

        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration, AppSettings appSettings, string migrationsAssembly)
        {
            // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            ConfigureIdentityServices(services, configuration);
            /* ConfigureIdentityServerServices(services, environment, appSettings, migrationsAssembly); */
        }

        private static void ConfigureIdentityServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<Models.UserX, IdentityRole<Guid>>(_ =>
                {
                    _.SignIn.RequireConfirmedAccount = true;

                    // Password settings.
                    _.Password.RequireDigit = true;
                    _.Password.RequireLowercase = true;
                    _.Password.RequireNonAlphanumeric = true;
                    _.Password.RequireUppercase = true;
                    _.Password.RequiredLength = 6;
                    _.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    _.Lockout.MaxFailedAccessAttempts = 5;
                    _.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    _.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    _.User.RequireUniqueEmail = false;
                })
            .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(_ =>
                {
                    _.LoginPath = $"/Account/Login";
                    _.LogoutPath = $"/Account/Logout";
                    _.AccessDeniedPath = $"/Account/AccessDenied";
                });
        }

        // https://github.com/IdentityServer/IdentityServer4.Demo
        // http://docs.identityserver.io/en/latest/topics/startup.html
        private static void ConfigureIdentityServerServices(IServiceCollection services, IWebHostEnvironment environment, AppSettings appSettings, string migrationsAssembly)
        {
            var builder = services.AddIdentityServer(_ =>
                {
                    _.Events.RaiseSuccessEvents = true;
                    _.Events.RaiseFailureEvents = true;
                    _.Events.RaiseErrorEvents = true;

                    _.Discovery.ShowIdentityScopes = true;
                    _.Discovery.ShowApiScopes = true;
                    _.Discovery.ShowClaims = true;
                    _.Discovery.ShowExtensionGrantTypes = true;
                    _.Discovery.CustomEntries.Add("api", "~/api");
                })
            // this adds the config data from DB (clients, resources)
            .AddConfigurationStore(_ =>
                {
                    _.DefaultSchema = appSettings.Database.SchemaName.IdentityServerConfiguration;
                    _.ConfigureDbContext = builder =>
                    builder.UseNpgsql(appSettings.Database.ConnectionString,
                        sql =>
                        {
                            /* sql.UseNodaTime(); */
                            sql.MigrationsAssembly(migrationsAssembly);
                        });
                })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(_ =>
                {
                    _.DefaultSchema = appSettings.Database.SchemaName.IdentityServerPersistedGrant;
                    _.ConfigureDbContext = builder =>
                      builder.UseNpgsql(appSettings.Database.ConnectionString,
                          sql =>
                          {
                              /* sql.UseNodaTime(); */
                              sql.MigrationsAssembly(migrationsAssembly);
                          });
                    // TODO enable token cleanup in production
                    // this enables automatic token cleanup. this is optional.
                    /* _.EnableTokenCleanup = true; */
                    /* _.TokenCleanupInterval = 30; // interval in seconds */
                })
              .AddSecretParser<JwtBearerClientAssertionSecretParser>()
              .AddSecretValidator<PrivateKeyJwtSecretValidator>() // https://identityserver4.readthedocs.io/en/latest/topics/secrets.html#beyond-shared-secrets
              .AddAspNetIdentity<Models.UserX>()
              .AddProfileService<ProfileService<Models.UserX>>();
            /* builder.AddApiAuthorization<UserX, ApplicationDbContext>(); */
            if (environment.IsDevelopment() || environment.IsEnvironment("test"))
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                // TODO https://identityserver4.readthedocs.io/en/latest/topics/startup.html#key-material
                throw new Exception("need to configure key material");
            }

            // https://identityserver4.readthedocs.io/en/latest/topics/apis.html#the-identityserver-authentication-handler
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
              .AddIdentityServerAuthentication(_ =>
                  {
                      // base-address of your identityserver
                      _.Authority = appSettings.Host;
                      _.RequireHttpsMetadata = !(environment.IsDevelopment() || environment.IsEnvironment("test"));

                      // name of the API resource
                      _.ApiName = ApiName;
                      _.ApiSecret = ApiSecret;

                      _.EnableCaching = true;
                      _.CacheDuration = TimeSpan.FromMinutes(10); // that's the default
                  })
            .AddLocalApi(_ =>
                {
                    _.ExpectedScope = ApiName;
                });
            services.AddAuthorization(_ =>
                {
                    _.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
                    {
                        policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                    });
                });
            // TODO? dotnet add package Microsoft.AspNetCore.Authentication.Cookies
            /* .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, _ => */
            /*    { */
            /*    }); */
        }

        public static void Configure(IApplicationBuilder app)
        {
            /* app.UseIdentity(); */
            /* app.UseIdentityServer(); */
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}