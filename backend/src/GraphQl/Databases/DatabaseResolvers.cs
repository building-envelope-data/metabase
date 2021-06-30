using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using Microsoft.Extensions.Logging;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
using HotChocolate;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Metabase.GraphQl.Databases
{
    public class DatabaseResolvers
    {
        private static readonly JsonSerializerOptions SerializerOptions =
                new()
                {
                    Converters = { new JsonStringEnumConverter(new ConstantCaseJsonNamingPolicy(), false)},
                    NumberHandling = JsonNumberHandling.Strict,
                    PropertyNameCaseInsensitive = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReadCommentHandling = JsonCommentHandling.Disallow,
                    IncludeFields = false,
                    IgnoreReadOnlyProperties = false,
                    IgnoreReadOnlyFields = true,
                    IgnoreNullValues = false
                }; //.SetupImmutableConverter();

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DatabaseResolvers> _logger;

        public DatabaseResolvers(
            IHttpClientFactory httpClientFactory,
            ILogger<DatabaseResolvers> logger
        )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<DataX.Data?> GetDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return await QueryDatabase<DataX.Data>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "Data.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        id,
                        timestamp,
                        locale,
                    },
                    operationName: "Data"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.OpticalData?> GetOpticalDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return await QueryDatabase<DataX.OpticalData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "OpticalDataFields.graphql",
                            "OpticalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        id,
                        timestamp,
                        locale,
                    },
                    operationName: "OpticalData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.HygrothermalData?> GetHygrothermalDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return await QueryDatabase<DataX.HygrothermalData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "HygrothermalDataFields.graphql",
                            "HygrothermalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        id,
                        timestamp,
                        locale,
                    },
                    operationName: "HygrothermalData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.CalorimetricData?> GetCalorimetricDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return await QueryDatabase<DataX.CalorimetricData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "CalorimetricDataFields.graphql",
                            "CalorimetricData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        id,
                        timestamp,
                        locale,
                    },
                    operationName: "CalorimetricData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.PhotovoltaicData?> GetPhotovoltaicDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return await QueryDatabase<DataX.PhotovoltaicData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "PhotovoltaicDataFields.graphql",
                            "PhotovoltaicData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        id,
                        timestamp,
                        locale,
                    },
                    operationName: "PhotovoltaicData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.DataConnection?> GetAllDataAsync(
            Data.Database database,
            DataX.DataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            /*TODO add `u`*/int? first,
            string? after,
            /*TODO add `u`*/int? last,
            string? before,
            CancellationToken cancellationToken
            )
        {
            return await QueryDatabase<DataX.DataConnection>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new {
                        where
                        // timestamp,
                        // locale,
                        // first,
                        // after,
                        // last,
                        // before
                    },
                    operationName: "AllData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        private sealed class AllOpticalDataData
        {
            public DataX.OpticalDataConnection AllOpticalData { get; set; } = default!;
        }

        public async Task<DataX.OpticalDataConnection?> GetAllOpticalDataAsync(
            Data.Database database,
            DataX.OpticalDataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            /*TODO add `u`*/int? first,
            string? after,
            /*TODO add `u`*/int? last,
            string? before,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<AllOpticalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "OpticalDataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllOpticalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new {
                        where
                        // timestamp,
                        // locale,
                        // first,
                        // after,
                        // last,
                        // before
                    },
                    operationName: "AllOpticalData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllOpticalData;
        }

        public async Task<DataX.HygrothermalDataConnection?> GetAllHygrothermalDataAsync(
            Data.Database database,
            DataX.HygrothermalDataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            /*TODO add `u`*/int? first,
            string? after,
            /*TODO add `u`*/int? last,
            string? before,
            CancellationToken cancellationToken
            )
        {
            return await QueryDatabase<DataX.HygrothermalDataConnection>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "HygrothermalDataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllHygrothermalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new {
                        where
                        // timestamp,
                        // locale,
                        // first,
                        // after,
                        // last,
                        // before
                    },
                    operationName: "AllHygrothermalData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.CalorimetricDataConnection?> GetAllCalorimetricDataAsync(
            Data.Database database,
            DataX.CalorimetricDataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            /*TODO add `u`*/int? first,
            string? after,
            /*TODO add `u`*/int? last,
            string? before,
            CancellationToken cancellationToken
            )
        {
            return await QueryDatabase<DataX.CalorimetricDataConnection>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "CalorimetricDataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllCalorimetricData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new {
                        where
                        // timestamp,
                        // locale,
                        // first,
                        // after,
                        // last,
                        // before
                    },
                    operationName: "AllCalorimetricData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<DataX.PhotovoltaicDataConnection?> GetAllPhotovoltaicDataAsync(
            Data.Database database,
            DataX.PhotovoltaicDataPropositionInput where,
            DateTime? timestamp,
            string? locale,
            /*TODO add `u`*/int? first,
            string? after,
            /*TODO add `u`*/int? last,
            string? before,
            CancellationToken cancellationToken
            )
        {
            return await QueryDatabase<DataX.PhotovoltaicDataConnection>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "PhotovoltaicDataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllPhotovoltaicData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new {
                        where
                        // timestamp,
                        // locale,
                        // first,
                        // after,
                        // last,
                        // before
                    },
                    operationName: "AllPhotovoltaicData"
                ),
                cancellationToken
            ).ConfigureAwait(false);
        }

        private static async Task<string> ConstructQuery(
            string[] fileNames
        )
        {
            return string.Join(
                Environment.NewLine,
                await Task.WhenAll(
                    fileNames.Select(fileName =>
                        File.ReadAllTextAsync($"GraphQl/Databases/Queries/{fileName}")
                    )
                ).ConfigureAwait(false)
            );
        }

        private async
          Task<TGraphQlResponse?>
          QueryDatabase<TGraphQlResponse>(
            Data.Database database,
            GraphQL.GraphQLRequest request,
            CancellationToken cancellationToken
            )
          where TGraphQlResponse : class
        {
            try
            {
                // https://github.com/graphql-dotnet/graphql-client/blob/47b4abfbfda507a91b5c62a18a9789bd3a8079c7/src/GraphQL.Client/GraphQLHttpResponse.cs
                // var response =
                //   (
                //    await CreateGraphQlClient(database)
                //    .SendQueryAsync<TGraphQlResponse>(
                //      request,
                //      cancellationToken
                //      )
                //    .ConfigureAwait(false)
                //    )
                //   .AsGraphQLHttpResponse();
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage =
                    await QueryGraphQl(
                        client,
                        database.Locator,
                        request
                    ).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogWarning($"Accessing the database {database.Locator} failed with status code {response.StatusCode}.");
                    return null;
                }
                using var graphQlResponseStream =
                    await httpResponseMessage.Content
                    .ReadAsStreamAsync(cancellationToken)
                    .ConfigureAwait(false);
                var deserializedGraphQlResponse =
                    await JsonSerializer.DeserializeAsync<GraphQL.GraphQLResponse<TGraphQlResponse>>(
                        graphQlResponseStream,
                        SerializerOptions,
                        cancellationToken
                    ).ConfigureAwait(false);
                if (deserializedGraphQlResponse is null)
                {
                    // TODO add details
                    _logger.LogWarning($"Failed to deserialize the GraphQL response received from the database {database.Locator}.");
                }
                // TODO What are `deserializedGraphQlResponse#Extensions`?
                if (deserializedGraphQlResponse?.Errors?.Length >= 1)
                {
                    // TODO Report errors to client? With error code `ErrorCodes.GraphQlRequestFailed`?
                    // TODO add details
                    _logger.LogWarning($"Accessing the database {database.Locator} failed with errors {JsonSerializer.Serialize(deserializedGraphQlResponse?.Errors)}");
                }
                return deserializedGraphQlResponse?.Data;
            }
            catch (GraphQLHttpRequestException e) // TODO Catch Http exceptions instead ...
            {
                _logger.LogError(e, $"Message: {e.Message}; Response Headers: {e.ResponseHeaders}; Content: {e.Content}");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private static string GraphQlLocationToString(
            GraphQL.GraphQLLocation location
            )
        {
            return $"{location.Line}:{location.Column}";
        }

        // private GraphQLHttpClient CreateGraphQlClient(
        //     Data.Database database
        //     )
        // {
        //     return new GraphQLHttpClient(
        //         new GraphQLHttpClientOptions { EndPoint = database.Locator },
        //         new SystemTextJsonSerializer(SerializerOptions),
        //         _httpClientFactory.CreateClient()
        //         );
        // }

        private static Task<HttpResponseMessage> QueryGraphQl(
            HttpClient httpClient,
            Uri locator,
            GraphQL.GraphQLRequest request
            )
        {
            return httpClient.PostAsync(
                locator,
                MakeJsonHttpContent(request)
            );
        }

        private static HttpContent MakeJsonHttpContent<TContent>(
            TContent content
            )
        {
            var result =
              new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes(
                  content,
                  SerializerOptions
                  )
                );
            result.Headers.ContentType =
              new MediaTypeHeaderValue("application/json");
            return result;
        }
    }
}