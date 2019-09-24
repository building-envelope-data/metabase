using Icon.Models;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;

// TODO ? Certificate authentication: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-3.0

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbContext")));

            // https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
            // services.AddDbContext<ApplicationDbContext>(options =>
            //   {
            //     options.UseNpgsql(Configuration.GetSection("DatabaseConfig")["PostgresSQL"]);
            //   });
            /* services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); */

            // TODO The following is in the file IdentityHostingStartup.cs now
            /* services.AddDefaultIdentity<ApplicationUser>() */
            /*                     .AddRoles<IdentityRole>() */
            /*     .AddEntityFrameworkStores<ApplicationDbContext>(); */

            // http://docs.identityserver.io/en/latest/topics/startup.html
            var builder = services.AddIdentityServer()
              // this adds the operational data from DB (codes, tokens, consents)
              .AddOperationalStore(options =>
                  {
                      options.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("ApplicationDbContext"));
                      // TODO enable token cleanup in production
                      // this enables automatic token cleanup. this is optional.
                      /* options.EnableTokenCleanup = true; */
                      /* options.TokenCleanupInterval = 30; // interval in seconds */
                  })
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
              .AddInMemoryApiResources(Config.GetApis())
              .AddInMemoryClients(Config.GetClients())
              .AddAspNetIdentity<ApplicationUser>();
            /* builder.AddApiAuthorization<ApplicationUser, ApplicationDbContext>(); */
            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            // Alternative: https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            services.AddAuthentication()
              .AddIdentityServerJwt();

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

            services.AddSwaggerDocument();

            services.AddRazorPages(
                options =>
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-3.0&tabs=visual-studio-code
                    options.RootDirectory = "/src/Pages";
                    /* options.Conventions.AuthorizeFolder("/MyPages/Admin"); */
                }
                );
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

            app.UseOpenApi();
            app.UseSwaggerUi3();
            // TODO https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
            /* app.UseSwaggeUi3(Startup.Assembly, settings => { */
            /* settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase; */
            /* settings.GeneratorSettings.DocumentProcessors.Add( */
            /*   new SecurityDefinitionAppender("oauth2", new SwaggerSecurityScheme */
            /*     { */
            /*     Type = SwaggerSecuritySchemeType.OAuth2, */
            /*     Flow = SwaggerOAuth2Flow.Implicit, */
            /*     AuthorizationUrl = "https://localhost:5001/connect/authorize", */
            /*     Scopes = new Dictionary<string, string> {{"api", "Icon (full access)"}} */
            /*     })); */
            /* settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2")); */
            /* settings.OAuth2Client = new OAuth2ClientSettings */
            /* { */
            /* ClientId = "api_swagger", */
            /* AppName = "Icon - Swagger" */
            /* }; */
            /* }); */


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