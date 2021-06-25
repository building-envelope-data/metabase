using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using Microsoft.Extensions.Logging;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
using HotChocolate;

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

        public Task<DataX.DataConnection?> GetAllOpticalDataAsync(
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
            return QueryDatabase<DataX.DataConnection>(
                database,
                new GraphQL.GraphQLRequest(
                    query: File.ReadAllText("GraphQl/Databases/AllOpticalData.graphql"),
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
                return response.Data;
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
                serializer: new SystemTextJsonSerializer()
                );
        }
    }
}