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

namespace Metabase.GraphQl.Databases
{
    public class DatabaseResolvers
    {
        private readonly ILogger<DatabaseResolvers> _logger;

        public DatabaseResolvers(
            ILogger<DatabaseResolvers> logger
        )
        {
            _logger = logger;
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

        private record AllDataVariables(
            DataX.OpticalDataPropositionInput Where
            // DateTime? Timestamp,
            // string? Locale,
            // /*TODO add `u`*/int? First,
            // string? After,
            // /*TODO add `u`*/int? Last,
            // string? Before
        );

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
            return await QueryDatabase<DataX.OpticalDataConnection>(
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
                    variables: new AllDataVariables(
                        where
                        // timestamp,
                        // locale,
                        // first,
                        // after,
                        // last,
                        // before
                    ),
                    operationName: "AllOpticalData"
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
            // https://github.com/graphql-dotnet/graphql-client/blob/47b4abfbfda507a91b5c62a18a9789bd3a8079c7/src/GraphQL.Client/GraphQLHttpResponse.cs
            try
            {
                var response =
                  (
                   await CreateGraphQlClient(database)
                   .SendQueryAsync<TGraphQlResponse>(
                     request,
                     cancellationToken
                     )
                   .ConfigureAwait(false)
                   )
                  .AsGraphQLHttpResponse();
                if (
                    response.StatusCode != System.Net.HttpStatusCode.OK ||
                    response.Errors?.Length >= 1 // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types#lifted-operators
                    )
                {
                    // TODO Report errors to client? With error code `ErrorCodes.GraphQlRequestFailed`?
                    _logger.LogWarning($"Accessing the database {database.Locator} failed with status code {response.StatusCode} and errors {(response.Errors is null ? "" : string.Join(", ", response.Errors.Select(GraphQlErrorToString)))}");
                    return null;
                }
                Console.WriteLine("bbbbbbbbbbb");
                Console.WriteLine(database.Locator);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Errors?.ToString());
                Console.WriteLine(JsonSerializer.Serialize(response.Data));
                return response.Data;
            }
            catch (GraphQLHttpRequestException e)
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

        private static string GraphQlErrorToString(
            GraphQL.GraphQLError error
            )
        {
            return $"GraphQlError(message: {error.Message}, locations: [{(error.Locations is null ? "" : string.Join(", ", error.Locations.Select(GraphQlLocationToString)))}], path: [{(error.Path is null ? "" : string.Join(", ", error.Path))}], extensions: {(error.Extensions is null ? "[]" : string.Join(", ", error.Extensions))})";
        }

        private static string GraphQlLocationToString(
            GraphQL.GraphQLLocation location
            )
        {
            return $"{location.Line}:{location.Column}";
        }

        private static GraphQLHttpClient CreateGraphQlClient(
            Data.Database database
            )
        {
            return new GraphQLHttpClient(
                endPoint: database.Locator.AbsoluteUri,
                serializer: new SystemTextJsonSerializer(
                    new JsonSerializerOptions
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
                    }
                    //.SetupImmutableConverter()
                    )
                );
        }
    }
}