// Inspired by https://hotchocolate.io/docs/aspnet#asp-net-core-

using HotChocolate;
using HotChocolate.AspNetCore;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;

namespace Icon.Configuration
{
    class GraphQl
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(
                SchemaBuilder.New());
                // .AddQueryType<QueryType>()
                // .AddType<CharacterType>())
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseGraphQL();
        }
    }
}