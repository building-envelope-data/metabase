using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Metabase.GraphQl.Databases;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;
using Metabase.Data.Extensions;
using Metabase.Enumerations;
using System.Collections.Generic;

namespace import
{
    public static class Program
    {
        public static async Task<int> Main(string[] commandLineArguments)
        {
            Console.WriteLine("About to import data from LBNL!");
            if (commandLineArguments.Length != 2)
            {
                Console.WriteLine("Two arguments are required: access token and environment");
                return 1;
            }
            var accessToken = commandLineArguments[0];
            var connectionString =
                commandLineArguments[1] == "production"
                ? "Host=database; Port=5432; Database=xbase; User Id=postgres; Passfile=/run/secrets/postgres_passwords; Maximum Pool Size=90;"
                : "Host=database; Port=5432; Database=xbase_development; User Id=postgres; Password=postgres; Maximum Pool Size=90;";
            Console.WriteLine($"Connecting to database {connectionString}.");
            var httpClientFactory = new HttpClientFactory();
            var resolvers = new DatabaseResolvers(
                httpClientFactory,
                new NullLogger<DatabaseResolvers>()
            );
            Console.WriteLine("Fetching all optical data from GraphQL endpoint.");
            var allOpticalData = await resolvers.GetAllOpticalDataAsync(
                new Database("LBNL", "lbnl", new Uri("https://igsdb-icon.herokuapp.com/icon_graphql/")),
                new Metabase.GraphQl.DataX.OpticalDataPropositionInput(null, null, null, null, null),
                null,
                null,
                null,
                null,
                null,
                null,
                default
                ).ConfigureAwait(false);
            if (allOpticalData is null)
            {
                return 2;
            }
            if (allOpticalData.Nodes is null)
            {
                return 3;
            }
            var httpClient = httpClientFactory.CreateClient();
            var dbContext = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseNpgsql(connectionString)
                    .UseSchemaName("metabase")
                    .Options
                );
            var iseInstitution = await dbContext.Institutions.SingleAsync(
                x => x.Name == "Fraunhofer ISE"
            ).ConfigureAwait(false);
            Console.WriteLine($"Fraunhofer ISE: {iseInstitution.Id}");
            Console.WriteLine("Fetching products from REST endpoint.");
            var indexRegex = new Regex(@"(\d+)\/$");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", accessToken);
            System.IO.Stream? contentStream = null;
            try
            {
                var response = await httpClient.GetAsync("https://igsdb-icon.herokuapp.com/api/v1/products/", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch product data: {response.StatusCode}.");
                    return 1;
                }
                contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to fetch product data: {e}.");
                return 1;
            }
            JsonDocument? data = null;
            try
            {
                data = await JsonDocument.ParseAsync(contentStream).ConfigureAwait(false);
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Failed to deserialize data: {e}.");
                return 1;
            }
            if (data is null)
            {
                Console.WriteLine("Failed to parse product data.");
                return 1;
            }
            var idToProduct = new Dictionary<int, JsonElement>();
            var root = data.RootElement;
            foreach (var product in root.EnumerateArray())
            {
                var productId = product.GetProperty("product_id").GetInt32();
                idToProduct.Add(productId, product);
            }
            Console.WriteLine("Inserting institutions and components into database.");
            foreach (var opticalData in allOpticalData.Nodes)
            {
                try
                {
                    var locator = opticalData.ResourceTree.Root.Value.Locator;
                    var match = indexRegex.Match(locator.AbsoluteUri);
                    if (match?.Success is not true)
                    {
                        Console.WriteLine($"Failed to extract product ID from URI {locator}.");
                        continue;
                    }
                    var productId = int.Parse(match.Groups[1].Value);
                    if (!idToProduct.ContainsKey(productId))
                    {
                        Console.WriteLine($"Product with id {productId} does not exist.");
                        continue;
                    }
                    var product = idToProduct.GetValueOrDefault(productId);
                    var productName = product.GetProperty("name").GetString();
                    var productDescription = product.GetProperty("short_description").GetString() ?? "";
                    var manufacturerName = product.GetProperty("manufacturer_name").GetString();
                    if (productName is null || productDescription is null || manufacturerName is null)
                    {
                        Console.WriteLine($"Some of the data {productName}, {productDescription}, and {manufacturerName} is null.");
                        return 1;
                    }
                    var institution = await dbContext.Institutions.SingleOrDefaultAsync(x => x.Name == manufacturerName).ConfigureAwait(false);
                    if (institution is null)
                    {
                        institution = new Institution(
                                name: manufacturerName,
                                abbreviation: null,
                                description: "",
                                websiteLocator: null,
                                null,
                                InstitutionState.OPERATIVE
                            )
                        {
                            ManagerId = iseInstitution.Id
                        };
                    }
                    var component =
                        new Component(
                            name: productName,
                            abbreviation: null,
                            description: productDescription,
                            availability: null,
                            categories: Array.Empty<ComponentCategory>()
                        )
                        { Id = opticalData.ComponentId };
                    component.ManufacturerEdges.Add(
                            new ComponentManufacturer
                            {
                                Institution = institution,
                                Pending = false
                            }
                        );
                    dbContext.Components.Add(component);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something failed: {e}");
                }
            }
            return 0;
        }

        public sealed class HttpClientFactory : IHttpClientFactory
        {
            private static readonly Lazy<HttpClient> _httpClientLazy = new(() => new HttpClient());

            public HttpClient CreateClient(string name)
            {
                return _httpClientLazy.Value;
            }
        }

    }
}
