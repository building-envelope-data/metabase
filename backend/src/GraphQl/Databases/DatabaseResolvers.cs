using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
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
                    Converters = { new JsonStringEnumConverter(new ConstantCaseJsonNamingPolicy(), false) },
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

        private sealed class DataData
        {
            public DataX.Data Data { get; set; } = default!;
        }

        public async Task<DataX.Data?> GetDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<DataData>(
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
            ).ConfigureAwait(false)
            )?.Data;
        }

        private sealed class OpticalDataData
        {
            public DataX.OpticalData OpticalData { get; set; } = default!;
        }

        public async Task<DataX.OpticalData?> GetOpticalDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<OpticalDataData>(
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
            ).ConfigureAwait(false)
            )?.OpticalData;
        }

        private sealed class HygrothermalDataData
        {
            public DataX.HygrothermalData HygrothermalData { get; set; } = default!;
        }

        public async Task<DataX.HygrothermalData?> GetHygrothermalDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<HygrothermalDataData>(
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
            ).ConfigureAwait(false)
            )?.HygrothermalData;
        }

        private sealed class CalorimetricDataData
        {
            public DataX.CalorimetricData CalorimetricData { get; set; } = default!;
        }

        public async Task<DataX.CalorimetricData?> GetCalorimetricDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<CalorimetricDataData>(
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
            ).ConfigureAwait(false)
            )?.CalorimetricData;
        }

        private sealed class PhotovoltaicDataData
        {
            public DataX.PhotovoltaicData PhotovoltaicData { get; set; } = default!;
        }

        public async Task<DataX.PhotovoltaicData?> GetPhotovoltaicDataAsync(
            Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<PhotovoltaicDataData>(
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
            ).ConfigureAwait(false)
            )?.PhotovoltaicData;
        }

        private sealed class AllDataData
        {
            public DataX.DataConnection AllData { get; set; } = default!;
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
            return (await QueryDatabase<AllDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where,
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllData;
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
                    variables: new
                    {
                        where,
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllOpticalData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllOpticalData;
        }

        private sealed class AllHygrothermalDataData
        {
            public DataX.HygrothermalDataConnection AllHygrothermalData { get; set; } = default!;
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
            return (await QueryDatabase<AllHygrothermalDataData>(
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
                    variables: new
                    {
                        where,
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllHygrothermalData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllHygrothermalData;
        }

        private sealed class AllCalorimetricDataData
        {
            public DataX.CalorimetricDataConnection AllCalorimetricData { get; set; } = default!;
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
            return (await QueryDatabase<AllCalorimetricDataData>(
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
                    variables: new
                    {
                        where,
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllCalorimetricData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllCalorimetricData;
        }

        private sealed class AllPhotovoltaicDataData
        {
            public DataX.PhotovoltaicDataConnection AllPhotovoltaicData { get; set; } = default!;
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
            return (await QueryDatabase<AllPhotovoltaicDataData>(
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
                    variables: new
                    {
                        where,
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllPhotovoltaicData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllPhotovoltaicData;
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
                var httpClient = _httpClientFactory.CreateClient();
                // For some reason `httpClient.PostAsJsonAsync` without `MakeJsonHttpContent` but with `SerializerOptions` results in `BadRequest` status code. It has to do with `JsonContent.Create` used within `PostAsJsonAsync` --- we also cannot use `JsonContent.Create` in `MakeJsonHttpContent`. What is happening here?
                var httpResponseMessage =
                    await httpClient.PostAsync(
                        database.Locator,
                        MakeJsonHttpContent(request),
                        cancellationToken
                    ).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogWarning($"Failed with status code {httpResponseMessage.StatusCode} to query the database {database.Locator} for {JsonSerializer.Serialize(request)}.");
                    return null;
                }
                // We could use `httpResponseMessage.Content.ReadFromJsonAsync<GraphQL.GraphQLResponse<TGraphQlResponse>>` which would make debugging more difficult though, https://docs.microsoft.com/en-us/dotnet/api/system.net.http.json.httpcontentjsonextensions.readfromjsonasync?view=net-5.0#System_Net_Http_Json_HttpContentJsonExtensions_ReadFromJsonAsync__1_System_Net_Http_HttpContent_System_Text_Json_JsonSerializerOptions_System_Threading_CancellationToken_
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
                    _logger.LogWarning($"Failed to deserialize the GraphQL response received from the database {database.Locator} for {JsonSerializer.Serialize(request)}.");
                }
                if (deserializedGraphQlResponse?.Errors?.Length >= 1)
                {
                    _logger.LogWarning($"Failed with errors {JsonSerializer.Serialize(deserializedGraphQlResponse?.Errors)} to query the database {database.Locator} for {JsonSerializer.Serialize(request)}");
                }
                return deserializedGraphQlResponse?.Data;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, $"Failed with status code {e.StatusCode} to request {database.Locator} for {JsonSerializer.Serialize(request)}.");
                throw;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, $"Failed to deserialize GraphQL response of request to {database.Locator} for {JsonSerializer.Serialize(request)}. The details given are {JsonSerializer.Serialize(e)}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to request {database.Locator} for {JsonSerializer.Serialize(request)} or failed to deserialize the response.");
                throw;
            }
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

        private static HttpContent MakeJsonHttpContent<TContent>(
            TContent content
            )
        {
            // For some reason using `JsonContent.Create<TContent>(content, null, SerializerOptions)` results in status code `BadRequest`.
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