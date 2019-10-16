using NSwag.Generation.Processors.Security;
using IdentityServer4.AccessTokenValidation;
using Icon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Microsoft.OpenApi;
using NSwag;
using IdentityServer4.AspNetIdentity;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Command = Icon.Infrastructure.Command;
using Query = Icon.Infrastructure.Query;
using Event = Icon.Infrastructure.Event;
using Domain = Icon.Domain;
using Component = Icon.Domain.Component;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;

namespace Icon.Configuration
{
    class Auth
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString, string migrationsAssembly, string host, IWebHostEnvironment environment, IConfiguration configuration)
        {
            {
                // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
                // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
                services.AddIdentity<Domain.User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders();
                // services.AddDbContext<ApplicationDbContext>(options =>
                //   {
                //     options.UseNpgsql(Configuration.GetSection("DatabaseConfig")["PostgresSQL"]);
                //   });
                /* services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); */

                // TODO The following is in the file IdentityHostingStartup.cs now
                /* services.AddDefaultIdentity<User>() */
                /*                     .AddRoles<IdentityRole>() */
                /*     .AddEntityFrameworkStores<ApplicationDbContext>(); */


                // http://docs.identityserver.io/en/latest/topics/startup.html
                var builder = services.AddIdentityServer(options =>
                    {
                        options.Events.RaiseSuccessEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseErrorEvents = true;

                        options.Discovery.ShowIdentityScopes = true;
                        options.Discovery.ShowApiScopes = true;
                        options.Discovery.ShowClaims = true;
                        options.Discovery.ShowExtensionGrantTypes = true;
                        options.Discovery.CustomEntries.Add("local_api", "~/localapi"); // TODO https://identityserver4.readthedocs.io/en/latest/topics/add_apis.html     ...   https://github.com/IdentityServer/IdentityServer4/blob/master/src/IdentityServer4/host/LocalApiController.cs
                    })
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                      builder.UseNpgsql(connectionString,
                          sql =>
                          {
                              /* sql.UseNodaTime(); */
                              sql.MigrationsAssembly(migrationsAssembly);
                          });
                    })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                      builder.UseNpgsql(connectionString,
                          sql =>
                          {
                              /* sql.UseNodaTime(); */
                              sql.MigrationsAssembly(migrationsAssembly);
                          });
                        // TODO enable token cleanup in production
                        // this enables automatic token cleanup. this is optional.
                        /* options.EnableTokenCleanup = true; */
                        /* options.TokenCleanupInterval = 30; // interval in seconds */
                    })
                .AddSecretParser<JwtBearerClientAssertionSecretParser>()
                  .AddSecretValidator<PrivateKeyJwtSecretValidator>() // https://identityserver4.readthedocs.io/en/latest/topics/secrets.html#beyond-shared-secrets
                  .AddAspNetIdentity<Domain.User>()
                  .AddProfileService<ProfileService<Domain.User>>();
                /* builder.AddApiAuthorization<User, ApplicationDbContext>(); */
                if (environment.IsDevelopment())
                {
                    builder.AddDeveloperSigningCredential();
                }
                else
                {
                    // TODO https://identityserver4.readthedocs.io/en/latest/topics/startup.html#key-material
                    throw new Exception("need to configure key material");
                }

                // Alternative: https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
                /* services.AddAuthentication() */
                /*   .AddIdentityServerJwt(); */
                /* services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) */
                /* .AddJwtBearer(options => */
                /* { */
                /*     // base-address of your identityserver */
                /*     options.Authority = "https://demo.identityserver.io"; */

                /*     // name of the API resource */
                /*     options.Audience = "api"; */
                /* }); */
                // https://identityserver4.readthedocs.io/en/latest/topics/apis.html#the-identityserver-authentication-handler
                services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                  .AddIdentityServerAuthentication(options =>
                      {
                          // base-address of your identityserver
                          options.Authority = host;
                          options.RequireHttpsMetadata = environment.IsProduction();

                          // name of the API resource
                          options.ApiName = Config.ApiName;
                          options.ApiSecret = Config.ApiSecret;

                          options.EnableCaching = true;
                          options.CacheDuration = TimeSpan.FromMinutes(10); // that's the default
                      });
                // TODO? dotnet add package Microsoft.AspNetCore.Authentication.Cookies
                /* .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => */
                /* 		{ */
                /* 		}); */

                services.Configure<IdentityOptions>(options =>
                        {
                            // Password settings.
                            options.Password.RequireDigit = true;
                            options.Password.RequireLowercase = true;
                            options.Password.RequireNonAlphanumeric = true;
                            options.Password.RequireUppercase = true;
                            options.Password.RequiredLength = 6;
                            options.Password.RequiredUniqueChars = 1;

                            // Lockout settings.
                            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                            options.Lockout.MaxFailedAccessAttempts = 5;
                            options.Lockout.AllowedForNewUsers = true;

                            // User settings.
                            options.User.AllowedUserNameCharacters =
                                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                            options.User.RequireUniqueEmail = false;
                        });

                services.AddTransient<IEmailSender, EmailSender>();
                services.Configure<AuthMessageSenderOptions>(configuration);

                services.AddLocalApiAuthentication();
            }


        }
        public static void Configure(IApplicationBuilder app)
        {
            /* app.UseIdentity(); */
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}