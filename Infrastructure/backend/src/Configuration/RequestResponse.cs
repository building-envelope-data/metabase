using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RazorViewEngine = Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine;
using RazorViewEngineOptions = Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions;
using HotChocolate;

namespace Infrastructure.Configuration
{
    public static class RequestResponse
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
            services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        }
    }
}