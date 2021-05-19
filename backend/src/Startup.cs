using System;
using System.Text.Json;
using System.Threading.Tasks;
using Metabase.Configuration;
using Metabase.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;

// TODO Certificate authentication: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth

namespace Metabase
{
    public sealed class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppSettings _appSettings;

        public Startup(
            IWebHostEnvironment environment,
            IConfiguration configuration
            )
        {
            _environment = environment;
            _appSettings = configuration.Get<AppSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AuthConfiguration.ConfigureServices(services, _environment, _appSettings);
            GraphQlConfiguration.ConfigureServices(services, _environment);
            ConfigureDatabaseServices(services);
            ConfigureMessageSenderServices(services);
            ConfigureRequestResponseServices(services);
            ConfigureSessionServices(services);
            services.AddHealthChecks();
            services.AddSingleton(_appSettings);
            services.AddSingleton(_environment);
            // services.AddDatabaseDeveloperPageExceptionFilter();
        }

        private static void ConfigureRequestResponseServices(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer#forwarded-headers-middleware-order
            services.Configure<ForwardedHeadersOptions>(_ =>
            {
                // TODO _.AllowedHosts = ...
                _.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer#forward-the-scheme-for-linux-and-non-iis-reverse-proxies
                _.KnownNetworks.Clear();
                _.KnownProxies.Clear();
            }
            );
            services.AddCors(_ =>
                _.AddDefaultPolicy(policy =>
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    )
                );
            services.AddControllersWithViews();
        }

        private void ConfigureMessageSenderServices(IServiceCollection services)
        {
            services.AddTransient<Services.IEmailSender>(serviceProvider =>
                new Services.EmailSender(
                    _appSettings.Email.SmtpHost,
                    _appSettings.Email.SmtpPort,
                    serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Services.EmailSender>>()
                )
            );
        }

        private static void ConfigureSessionServices(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state#session-state
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

        private void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<Data.ApplicationDbContext>(options =>
                {
                    options
                    .UseNpgsql(_appSettings.Database.ConnectionString)
                    .UseSchemaName(_appSettings.Database.SchemaName)
                    .UseOpenIddict();
                    /* .UseNodaTime() */ // https://www.npgsql.org/efcore/mapping/nodatime.html
                    if (!_environment.IsProduction())
                    {
                        options
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                    }
                }
                );
            // Database context as services are used by `Identity` and `OpenIddict`.
            services.AddDbContext<Data.ApplicationDbContext>(
                (services, options) =>
                {
                    if (!_environment.IsProduction())
                    {
                        options
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                    }
                    services
                    .GetRequiredService<IDbContextFactory<Data.ApplicationDbContext>>()
                    .CreateDbContext();
                },
                ServiceLifetime.Transient
                );
        }

        public void Configure(IApplicationBuilder app)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/
            if (_environment.IsDevelopment() || _environment.IsEnvironment("test"))
            {
                app.UseDeveloperExceptionPage();
                // app.UseMigrationsEndPoint();
                // Forwarded Headers Middleware must run before other middleware except diagnostics and error handling middleware. In particular before HSTS middleware.
                // See https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer#other-proxy-server-and-load-balancer-scenarios
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // Forwarded Headers Middleware must run before other middleware except diagnostics and error handling middleware. In particular before HSTS middleware.
                // See https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer#other-proxy-server-and-load-balancer-scenarios
                app.UseForwardedHeaders();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // ASP.NET advices to not use HSTS for APIs, see the warning on
                // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
                // app.UseHsts(); // Done by NGINX, see https://www.nginx.com/blog/http-strict-transport-security-hsts-and-nginx/
            }
            // app.UseStatusCodePages();
            // app.UseHttpsRedirection(); // Done by NGINX
            app.UseSerilogRequestLogging();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            // TODO Do we really want this? See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-5.0
            app.UseRequestLocalization(_ =>
            {
                _.AddSupportedCultures("en-US", "de-DE");
                _.AddSupportedUICultures("en-US", "de-DE");
                _.SetDefaultCulture("en-US");
            });
            app.UseCors();
            // app.UseCertificateForwarding(); // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0#other-web-proxies
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            // app.UseResponseCompression(); // Done by Nginx
            // app.UseResponseCaching(); // Done by Nginx
            app.UseHealthChecks(
                "/health",
                new HealthCheckOptions
                {
                    ResponseWriter = JsonResponseWriter
                }
                );
            /* app.UseWebSockets(); */
            app.UseEndpoints(_ =>
            {
                _.MapGraphQL();
                _.MapControllers();
            });
        }

        private async Task JsonResponseWriter(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                new
                {
                    Status = report.Status.ToString()
                },
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
                ).ConfigureAwait(false);
        }
    }
}