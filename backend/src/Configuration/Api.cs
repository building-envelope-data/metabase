using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Icon.Data;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using WebPWrecover.Services;
using Command = Icon.Infrastructure.Command;
using Event = Icon.Events;
using Models = Icon.Models;
using Query = Icon.Infrastructure.Query;

namespace Icon.Configuration
{
    public sealed class Api
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(document =>
                {
                    document.DocumentName = "icon";

                    // https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware#enable-authorization-in-generator-and-swagger-ui
                    // See also https://github.com/dvanherten/swagger-identityserver
                    // OAuth2
                    document.AddSecurity(
                        "bearer",
                        Enumerable.Empty<string>(),
                        new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.OAuth2,
                            Description = "OAuth2 authentication",
                            Flow = OpenApiOAuth2Flow.Implicit,
                            Flows = new OpenApiOAuthFlows()
                            {
                                Implicit = new OpenApiOAuthFlow()
                                {
                                    Scopes = new Dictionary<string, string>
                              {
                                { "api", "Read and write access to protected resources" },
                                /* { "read", "Read access to protected resources" }, */
                                /* { "write", "Write access to protected resources" } */
                              },
                                    AuthorizationUrl = "https://localhost:5001/connect/authorize",
                                    TokenUrl = "https://localhost:5001/connect/token"
                                },
                            }
                        });

                    document.OperationProcessors.Add(
                      new AspNetCoreOperationSecurityScopeProcessor("bearer"));
                    // Other types of auth*:
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

        public static void Configure(IApplicationBuilder app)
        {
            app.UseOpenApi(); // serve OpenAPI specification documents
            app.UseSwaggerUi3(settings =>
                    {
                        // https://www.scottbrady91.com/Identity-Server/ASPNET-Core-Swagger-UI-Authorization-using-IdentityServer4
                        settings.OAuth2Client = new OAuth2ClientSettings
                        {
                            ClientId = "swagger",
                            ClientSecret = Auth.ApiSecret,
                            AppName = "icon",
                            Realm = "icon",
                            AdditionalQueryStringParameters = { }
                        };
                    }); // serve Swagger UI
            app.UseReDoc(); // serve ReDoc UI
        }
    }
}