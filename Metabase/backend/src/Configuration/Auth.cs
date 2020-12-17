using System;
/* using IdentityServer4; */
/* using IdentityServer4.AccessTokenValidation; */
/* using IdentityServer4.AspNetIdentity; */
/* using IdentityServer4.Validation; */
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
/* using Microsoft.AspNetCore.Identity.UI.Services; */
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Metabase.Configuration
{
    public abstract class Auth
    {
        public static readonly string ApiName = "api";
        public static readonly string ApiSecret = "secret"; // TODO Put in environment variable.

        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            Infrastructure.AppSettings appSettings,
            string migrationsAssembly
            )
        {
            // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
          ConfigureIdentityServices(services, environment, configuration, appSettings);
            /* ConfigureIdentityServerServices(services, environment, appSettings, migrationsAssembly); */
        }

        private static void ConfigureIdentityServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            Infrastructure.AppSettings appSettings
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
                    _.Password.RequiredLength = 6;
                    _.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    _.Lockout.MaxFailedAccessAttempts = 5;
                    _.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    _.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    _.User.RequireUniqueEmail = true;
                })
              .AddEntityFrameworkStores<Data.ApplicationDbContext>()
              .AddDefaultTokenProviders();

            // Making services that were designed to be scoped transient is troublesome.
            /* services.AddTransient<IUserStore<Data.User>, UserStore<Data.User, Data.Role, Data.ApplicationDbContext, Guid, Data.UserClaim, Data.UserRole, Data.UserLogin, Data.UserToken, Data.RoleClaim>>(); */
            /* services.AddTransient<IRoleStore<Data.Role>, RoleStore<Data.Role, Data.ApplicationDbContext, Guid, Data.UserRole, Data.RoleClaim>>(); */
            /* services.AddTransient<UserManager<Data.User>>(); */
            /* services.AddTransient<IUserValidator<Data.User>, UserValidator<Data.User>>(); */
            /* services.AddTransient<RoleManager<Data.Role>>(); */
            /* services.AddTransient<IRoleValidator<Data.Role>, RoleValidator<Data.Role>>(); */
            /* services.AddTransient<SignInManager<Data.User>>(); */

            /* services.ConfigureApplicationCookie(_ => */
            /*     { */
            /*         _.LoginPath = "/Account/Login"; */
            /*         _.LogoutPath = "/Account/Logout"; */
            /*         _.AccessDeniedPath = "/Account/AccessDenied"; */
            /*     }); */

            // https://identityserver4.readthedocs.io/en/latest/topics/apis.html#the-identityserver-authentication-handler
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(
                  JwtBearerDefaults.AuthenticationScheme,
                  options => configuration.Bind("JwtSettings", options)
                  );
              /* .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => configuration.Bind("CookieSettings", options)); */
            services.AddAuthorization();
            /* services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme); */
            /*   .AddIdentityServerAuthentication(_ => */
            /*       { */
            /*           // base-address of your identityserver */
            /*           _.Authority = appSettings.Host; */
            /*           _.RequireHttpsMetadata = !(environment.IsDevelopment() || environment.IsEnvironment("test")); */

            /*           // name of the API resource */
            /*           _.ApiName = ApiName; */
            /*           _.ApiSecret = ApiSecret; */

            /*           _.EnableCaching = true; */
            /*           _.CacheDuration = TimeSpan.FromMinutes(10); // that's the default */
            /*       }) */
            /* .AddLocalApi(_ => _.ExpectedScope = ApiName); */
            /* services.AddAuthorization(_ => */
            /*     { */
            /*         _.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy => */
            /*         { */
            /*             policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme); */
            /*             policy.RequireAuthenticatedUser(); */
            /*         }); */
            /*     }); */

            // TODO? dotnet add package Microsoft.AspNetCore.Authentication.Cookies
            /* .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, _ => */
            /*    { */
            /*    }); */
        }

        // https://github.com/IdentityServer/IdentityServer4.Demo
        // http://docs.identityserver.io/en/latest/topics/startup.html
        // private static void ConfigureIdentityServerServices(
        //     IServiceCollection services,
        //     IWebHostEnvironment environment,
        //     Metabase.AppSettings appSettings, string migrationsAssembly
        //     )
        // {
        //     var builder = services.AddIdentityServer(_ =>
        //         {
        //             _.Events.RaiseSuccessEvents = true;
        //             _.Events.RaiseFailureEvents = true;
        //             _.Events.RaiseErrorEvents = true;

        //             _.Discovery.ShowIdentityScopes = true;
        //             _.Discovery.ShowApiScopes = true;
        //             _.Discovery.ShowClaims = true;
        //             _.Discovery.ShowExtensionGrantTypes = true;
        //             _.Discovery.CustomEntries.Add("api", "~/api");
        //         })
        //     // this adds the config data from DB (clients, resources)
        //     .AddConfigurationStore(_ =>
        //         {
        //             _.DefaultSchema = appSettings.Database.SchemaName.IdentityServerConfiguration;
        //             _.ConfigureDbContext = builder =>
        //             builder.UseNpgsql(appSettings.Database.ConnectionString,
        //                 sql =>
        //                 {
        //                     /* sql.UseNodaTime(); */
        //                     sql.MigrationsAssembly(migrationsAssembly);
        //                 });
        //         })
        //     // this adds the operational data from DB (codes, tokens, consents)
        //     .AddOperationalStore(_ =>
        //         {
        //             _.DefaultSchema = appSettings.Database.SchemaName.IdentityServerPersistedGrant;
        //             _.ConfigureDbContext = builder =>
        //               builder.UseNpgsql(appSettings.Database.ConnectionString,
        //                   sql =>
        //                   {
        //                       /* sql.UseNodaTime(); */
        //                       sql.MigrationsAssembly(migrationsAssembly);
        //                   });
        //             // TODO enable token cleanup in production
        //             // this enables automatic token cleanup. this is optional.
        //             /* _.EnableTokenCleanup = true; */
        //             /* _.TokenCleanupInterval = 30; // interval in seconds */
        //         })
        //       .AddSecretParser<JwtBearerClientAssertionSecretParser>()
        //       .AddSecretValidator<PrivateKeyJwtSecretValidator>() // https://identityserver4.readthedocs.io/en/latest/topics/secrets.html#beyond-shared-secrets
        //       .AddAspNetIdentity<Data.User>()
        //       .AddProfileService<ProfileService<Data.User>>();
        //     /* builder.AddApiAuthorization<User, ApplicationDbContext>(); */
        //     if (environment.IsDevelopment() || environment.IsEnvironment("test"))
        //     {
        //         builder.AddDeveloperSigningCredential();
        //     }
        //     else
        //     {
        //         // TODO https://identityserver4.readthedocs.io/en/latest/topics/startup.html#key-material
        //         throw new Exception("need to configure key material");
        //     }
        // }

        public static void Configure(IApplicationBuilder app)
        {
            /* app.UseIdentity(); */
            /* app.UseIdentityServer(); */
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
