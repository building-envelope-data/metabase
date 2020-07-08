using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Errors = Infrastructure.Errors;
using RazorViewEngine = Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine;
using RazorViewEngineOptions = Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions;

namespace Metabase.Configuration
{
    public sealed class RequestResponse
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            /* services.Configure<ForwardedHeadersOptions>(options => */
            /*     { */
            /*     options.KnownProxies.Add(IPAddress.Parse("10.0.0.100")); */
            /*     }); */

            services.AddResponseCompression();
            // add CORS policy for non-IdentityServer endpoints
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.0
            services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

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

            // Change Default Location Of Views And Razor Pages In ASP.NET Core
            // http://www.binaryintellect.net/articles/c50d3f14-7048-4b4f-84f4-1b28cb0f9d96.aspx
            services.AddControllersWithViews();
            services.Configure<RazorViewEngineOptions>(o =>
                {
                    o.ViewLocationFormats.Clear();
                    o.ViewLocationFormats.Add
            ("/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    o.ViewLocationFormats.Add
            ("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                });
            services.AddRazorPages(
                options =>
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-3.0&tabs=visual-studio-code
                    options.RootDirectory = "/Pages";
                    /* options.Conventions.AuthorizeFolder("/MyPages/Admin"); */
                }
                );
        }

        public static void ConfigureRouting(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment() || environment.IsEnvironment("test"))
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // ASP.NET advices to not use HSTS for APIs, see the warning on
                // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio
                app.UseHsts();
            }

            // Forwarded Headers Middleware should run before other middleware except diagnostics and error handling middleware.
            // See https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-3.1#use-a-reverse-proxy-server
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            }
                );

            app.UseStatusCodePages();

            // ASP.NET advices to not use HTTPS redirection for APIs, see the
            // warning on https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio
            // TODO Use `app.UseHttpsRedirection();`. The problem right now is
            // that requests from within the container to the container itself
            // fails with the exceptions
            // System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception.
            // ---> System.Security.Authentication.AuthenticationException: The remote certificate is invalid according to the validation procedure.
            // Although, we added a self-signed root certificate to the
            // container.

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(Auth.ApiName);
            app.UseResponseCompression(); // TODO Using the server-based compression of, for example, Nginx is much faster. See https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-3.0
        }

        public static void ConfigureEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
                    {
                        /* endpoints.MapHealthChecks("/health").RequireAuthorization(); // TODO Add healtch check services as described in https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0 */
                        endpoints.MapControllers();
                        endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}