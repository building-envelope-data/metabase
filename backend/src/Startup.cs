using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HotChocolate.AspNetCore;
using Metabase.Configuration;
using Metabase.Data.Extensions;
using Metabase.GraphQl.Databases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Serilog;

namespace Metabase
{
    public sealed class Startup
    {
        private const string GraphQlCorsPolicy = "GraphQlCorsPolicy";

        private readonly IWebHostEnvironment _environment;
        private readonly AppSettings _appSettings;

        public Startup(
            IWebHostEnvironment environment,
            IConfiguration configuration
            )
        {
            _environment = environment;
            _appSettings = configuration.Get<AppSettings>() ?? throw new InvalidOperationException("Failed to get application settings from configuration.");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AuthConfiguration.ConfigureServices(services, _environment, _appSettings);
            GraphQlConfiguration.ConfigureServices(services, _environment);
            ConfigureDatabaseServices(services);
            ConfigureMessageSenderServices(services);
            ConfigureRequestResponseServices(services);
            ConfigureSessionServices(services);
            services.AddAntiforgery(_ =>
            {
                _.HeaderName = "X-XSRF-TOKEN";
            });
            services
                .AddDataProtection()
                .PersistKeysToDbContext<Data.ApplicationDbContext>();
            ConfigureHttpClientServices(services);
            services.AddHttpContextAccessor();
            services
                .AddHealthChecks()
                .AddDbContextCheck<Data.ApplicationDbContext>();
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
                _.AddPolicy(
                    name: GraphQlCorsPolicy,
                    policy =>
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
                    var dataSourceBuilder = new NpgsqlDataSourceBuilder(_appSettings.Database.ConnectionString);
                    // Enumerations registered as below are not picked up by
                    // the tool `dotnet ef` when creating migrations. We thus
                    // register enumerations in `ApplicationDbContext`.
                    // dataSourceBuilder.MapEnum<Enumerations.ComponentCategory>();
                    // dataSourceBuilder.MapEnum<Enumerations.InstitutionRepresentativeRole>();
                    // dataSourceBuilder.MapEnum<Enumerations.InstitutionState>();
                    // dataSourceBuilder.MapEnum<Enumerations.MethodCategory>();
                    // dataSourceBuilder.MapEnum<Enumerations.Standardizer>();
                    options
                    .UseNpgsql(dataSourceBuilder.Build() /*, optionsBuilder => optionsBuilder.UseNodaTime() */)
                    .UseSchemaName(_appSettings.Database.SchemaName)
                    .UseOpenIddict();
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

        private static void ConfigureHttpClientServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient(QueryingDatabases.DATABASE_HTTP_CLIENT);
            // var httpClientBuilder = services.AddHttpClient(QueryingDatabases.DATABASE_HTTP_CLIENT);
            // if (!_environment.IsProduction())
            // {
            //     httpClientBuilder.ConfigurePrimaryHttpMessageHandler(_ =>
            //         new HttpClientHandler
            //         {
            //             ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            //         }
            //     );
            // }
        }

        public void Configure(IApplicationBuilder app)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/
            if (_environment.IsDevelopment() || _environment.IsEnvironment(Program.TestEnvironment))
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
            app.UseCookiePolicy(); // [SameSite cookies](https://learn.microsoft.com/en-us/aspnet/core/security/samesite)
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
            /* app.UseWebSockets(); */
            app.UseEndpoints(_ =>
            {
                _.MapGraphQL()
                .WithOptions(
                    // https://chillicream.com/docs/hotchocolate/server/middleware
                    new GraphQLServerOptions
                    {
                        EnableSchemaRequests = true,
                        EnableGetRequests = false,
                        // AllowedGetOperations = AllowedGetOperations.Query
                        EnableMultipartRequests = false,
                        Tool = {
                            DisableTelemetry = true,
                            Enable = true, // _environment.IsDevelopment()
                            IncludeCookies = false,
                            GraphQLEndpoint = "/graphql",
                            HttpMethod = DefaultHttpMethod.Post,
                            Title = "GraphQL"
                        }
                    }
                )
                .RequireCors(GraphQlCorsPolicy);
                _.MapControllers();
                _.MapHealthChecks("/health",
                    new HealthCheckOptions
                    {
                        ResponseWriter = WriteJsonResponse
                    }
                );
            });
        }

        // Inspired by https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0#customize-output
        private static Task WriteJsonResponse(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            var options = new JsonWriterOptions { Indented = true };
            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", healthReport.Status.ToString());
                jsonWriter.WriteString("duration", healthReport.TotalDuration.ToString());
                jsonWriter.WriteStartObject("results");
                foreach (var healthReportEntry in healthReport.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key);
                    jsonWriter.WriteString("status",
                        healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteString("description",
                        healthReportEntry.Value.Description);
                    jsonWriter.WriteString("duration",
                        healthReportEntry.Value.Duration.ToString());
                    jsonWriter.WriteStartArray("tags");
                    foreach (var tag in healthReportEntry.Value.Tags)
                    {
                        jsonWriter.WriteStringValue(tag);
                    }
                    jsonWriter.WriteEndArray();
                    var exception = healthReportEntry.Value.Exception;
                    if (exception is not null)
                    {
                        jsonWriter.WriteStartObject("exception");
                        jsonWriter.WriteString("message", exception.Message);
                        if (exception.StackTrace is not null)
                        {
                            jsonWriter.WriteString("stackTrace", exception.StackTrace);
                        }
                        if (exception.InnerException is not null)
                        {
                            jsonWriter.WriteString("innerException", exception.InnerException.ToString());
                        }
                        if (exception.Source is not null)
                        {
                            jsonWriter.WriteString("source", exception.Source);
                        }
                        if (exception.TargetSite is not null)
                        {
                            jsonWriter.WriteString("targetSite", exception.TargetSite.ToString());
                        }
                        jsonWriter.WriteEndObject();
                    }
                    jsonWriter.WriteStartObject("data");
                    foreach (var item in healthReportEntry.Value.Data)
                    {
                        jsonWriter.WritePropertyName(item.Key);
                        JsonSerializer.Serialize(jsonWriter, item.Value,
                            item.Value?.GetType() ?? typeof(object));
                    }
                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                }
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }
            return context.Response.WriteAsync(
                Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }
}