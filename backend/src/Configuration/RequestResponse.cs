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
using RazorViewEngineOptions = Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions;
using RazorViewEngine = Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine;

namespace Icon.Configuration
{
    class RequestResponse
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            // add CORS policy for non-IdentityServer endpoints
            services.AddCors(options =>
            {
                options.AddPolicy(Auth.ApiName, policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

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
            ("/src/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                    o.ViewLocationFormats.Add
            ("/src/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                });
            services.AddRazorPages(
                options =>
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-3.0&tabs=visual-studio-code
                    options.RootDirectory = "/src/Pages";
                    /* options.Conventions.AuthorizeFolder("/MyPages/Admin"); */
                }
                );
        }

        public static void ConfigureRouting(IApplicationBuilder app, bool isDevelopmentEnvironment)
        {

            if (isDevelopmentEnvironment)
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