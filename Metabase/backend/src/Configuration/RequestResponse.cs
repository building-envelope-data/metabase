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
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
            services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
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
        }
    }
}