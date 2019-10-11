using Icon.Models;
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
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;

// TODO ? Certificate authentication: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-3.0
// TODO Use NodaTime (see commented code below; confer https://www.npgsql.org/efcore/mapping/nodatime.html).
//      There are problems with the generated migrations of the identity server.
//      Maybe https://github.com/etiennemtl/identity-npsql-nodatime helps.
//      The error was `System.InvalidCastException: Can't write CLR type System.DateTime with handler type TimestampHandler`
//      See also https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/568
//      https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/648
// TODO API versioning: https://github.com/RicoSuter/NSwag/issues/2118

// OpenIddict is another implementation of OpenId Connect

// IdentityServer4 Demo: https://demo.identityserver.io/
//											 https://github.com/IdentityServer/IdentityServer4.Demo/blob/master/src/IdentityServer4Demo/Config.cs

// TODO IdentityServer4 Admin UI. Available options are:
//			https://github.com/skoruba/IdentityServer4.Admin
//			https://github.com/brunohbrito/JPProject.IdentityServer4.AdminUI
//			https://github.com/zarxor/IdentityServer4.OpenAdmin

// TODO For client libraries use https://identitymodel.readthedocs.io/en/latest/

// TODO Swagger UI for IdentityServer4 endpoints, follow `https://github.com/IdentityServer/IdentityServer4/issues/2286`

namespace Icon
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        private string GetHost()
        {
            return "https://icon.com"; // TODO Read from environment.
        }

        private string GetConnectionString()
        {
            return Configuration.GetConnectionString("ApplicationDbContext");
        }

        private string GetMigrationsAssembly()
        {
            return typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabaseServices(services);
            ConfigureSessionServices(services);
            ConfigureRequestResponseServices(services);
            ConfigureApiServices(services);
            ConfigureAuthServices(services);
        }

        private void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(GetConnectionString() /* , o => o.UseNodaTime() */)
                  .EnableSensitiveDataLogging(Configuration.GetValue<bool>("Logging:EnableSqlParameterLogging"))
                );
        }

        private void ConfigureSessionServices(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-3.0#session-state
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
                {
                    // Set a short timeout for easy testing.
                    options.IdleTimeout = TimeSpan.FromSeconds(10);
                    options.Cookie.HttpOnly = true;
                    // Make the session cookie essential
                    options.Cookie.IsEssential = true;
                });
        }

        private void ConfigureRequestResponseServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            // Using `AddControllersAsServices` makes controller custructors
            // being validated on startup with respect to the dependency
            // injection service provider. For details see
            // https://andrewlock.net/new-in-asp-net-core-3-service-provider-validation/#1-controller-constructor-dependencies-aren-t-checked
            services.AddControllers()
              .AddControllersAsServices()
              .ConfigureApiBehaviorOptions(
                  options =>
                  {
                      options.SuppressConsumesConstraintForFormFileParameters = true; // https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.0#multipartform-data-request-inference
                      options.SuppressInferBindingSourcesForParameters = true; // https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.0#disable-inference-rules
                                                                               // options.SuppressModelStateInvalidFilter = true;
                                                                               // options.SuppressMapClientErrors = true;
                                                                               // options.ClientErrorMapping[404].Link =
                                                                               //     "https://httpstatuses.com/404";
                  }
                  );

            services.AddRazorPages(
                options =>
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-3.0&tabs=visual-studio-code
                    options.RootDirectory = "/src/Pages";
                    /* options.Conventions.AuthorizeFolder("/MyPages/Admin"); */
                }
                );
        }

        private void ConfigureApiServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(document =>
                {
                    document.DocumentName = "OpenAPI 3";

                    // https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware#enable-authorization-in-generator-and-swagger-ui
                    // See also https://github.com/dvanherten/swagger-identityserver
                    // OAuth2
                    document.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                    {
                        // TODO Add proper values. Also below for `UseSwaggerUi3`
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Description = "My Authentication",
                        Flow = OpenApiOAuth2Flow.Implicit,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                Scopes = new Dictionary<string, string>
                    {
                  { "read", "Read access to protected resources" },
                  { "write", "Write access to protected resources" }
                    },
                                AuthorizationUrl = "https://localhost:5001/connect/authorize",
                                TokenUrl = "https://localhost:5001/connect/token"
                            },
                        }
                    });

                    document.OperationProcessors.Add(
                      new AspNetCoreOperationSecurityScopeProcessor("bearer"));

                    // API Key
                    /* document.AddSecurity("apikey", Enumerable.Empty<string>(), new OpenApiSecurityScheme */
                    /*     { */
                    /*     Type = OpenApiSecuritySchemeType.ApiKey, */
                    /*     Name = "api_key", */
                    /*     In = OpenApiSecurityApiKeyLocation.Header */
                    /*     }); */

                    // JWT
                    /* document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme */
                    /*     { */
                    /*     Type = OpenApiSecuritySchemeType.ApiKey, */
                    /*     Name = "Authorization", */
                    /*     In = OpenApiSecurityApiKeyLocation.Header, */
                    /*     Description = "Type into the textbox: Bearer {your JWT token}." */
                    /*     }); */

                });
        }

        private void ConfigureAuthServices(IServiceCollection services)
        {
            // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
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
                  builder.UseNpgsql(GetConnectionString(),
                      sql =>
                      {
                          /* sql.UseNodaTime(); */
                          sql.MigrationsAssembly(GetMigrationsAssembly());
                      });
                })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                  builder.UseNpgsql(GetConnectionString(),
                      sql =>
                      {
                          /* sql.UseNodaTime(); */
                          sql.MigrationsAssembly(GetMigrationsAssembly());
                      });
                    // TODO enable token cleanup in production
                    // this enables automatic token cleanup. this is optional.
                    /* options.EnableTokenCleanup = true; */
                    /* options.TokenCleanupInterval = 30; // interval in seconds */
                })
            .AddSecretParser<JwtBearerClientAssertionSecretParser>()
              .AddSecretValidator<PrivateKeyJwtSecretValidator>() // https://identityserver4.readthedocs.io/en/latest/topics/secrets.html#beyond-shared-secrets
              .AddAspNetIdentity<User>()
              .AddProfileService<ProfileService<User>>();
            /* builder.AddApiAuthorization<User, ApplicationDbContext>(); */
            if (Environment.IsDevelopment())
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
                      options.Authority = GetHost();
                      options.RequireHttpsMetadata = Environment.IsProduction();

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
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddLocalApiAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.0
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePages();

            // ASP.NET advices to not use HTTPS redirection for APIs, see the
            // warning on https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio
            app.UseHttpsRedirection();

            app.UseResponseCompression(); // TODO Using the server-based compression of, for example, Nginx is much faster. See https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-3.0
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseCors();

            app.UseOpenApi(); // serve OpenAPI specification documents
            app.UseSwaggerUi3(settings =>
                    {
                        // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
                        settings.OAuth2Client = new OAuth2ClientSettings
                        {
                            ClientId = "api_swagger",
                            ClientSecret = Config.ApiSecret,
                            AppName = "my_app",
                            Realm = "my_realm",
                            AdditionalQueryStringParameters =
                    {
                            { "foo", "bar" }
                    }
                        };
                    }); // serve Swagger UI
            app.UseReDoc(); // serve ReDoc UI


            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
                    {
                        /* endpoints.MapHealthChecks("/health").RequireAuthorization(); // TODO Add healtch check services as described in https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0 */
                        endpoints.MapControllers();
                        endpoints.MapDefaultControllerRoute();
                        endpoints.MapRazorPages();
                    });
        }
    }
}