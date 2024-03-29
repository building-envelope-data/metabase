using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using Metabase.Authorization;

namespace Metabase.GraphQl.Databases
{
    public sealed class DatabaseResolvers
    {
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

        public Task<bool> GetCanCurrentUserUpdateNodeAsync(
          [Parent] Data.Database database,
          ClaimsPrincipal claimsPrincipal,
          [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
          Data.ApplicationDbContext context,
          CancellationToken cancellationToken
        )
        {
            return DatabaseAuthorization.IsAuthorizedToUpdate(claimsPrincipal, database.Id, userManager, context, cancellationToken);
        }

        public Task<bool> GetCanCurrentUserVerifyNodeAsync(
          [Parent] Data.Database database,
          ClaimsPrincipal claimsPrincipal,
          [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
          Data.ApplicationDbContext context,
          CancellationToken cancellationToken
        )
        {
            return DatabaseAuthorization.IsAuthorizedToVerify(claimsPrincipal, database.Id, userManager, context, cancellationToken);
        }

        private sealed class DataData
        {
            public DataX.Data Data { get; set; } = default!;
        }

        public async Task<DataX.IData?> GetDataAsync(
            [Parent] Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<DataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.Data;
        }

        private sealed class OpticalDataData
        {
            public DataX.OpticalData OpticalData { get; set; } = default!;
        }

        public async Task<DataX.OpticalData?> GetOpticalDataAsync(
            [Parent] Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<OpticalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.OpticalData;
        }

        private sealed class HygrothermalDataData
        {
            public DataX.HygrothermalData HygrothermalData { get; set; } = default!;
        }

        public async Task<DataX.HygrothermalData?> GetHygrothermalDataAsync(
            [Parent] Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<HygrothermalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.HygrothermalData;
        }

        private sealed class CalorimetricDataData
        {
            public DataX.CalorimetricData CalorimetricData { get; set; } = default!;
        }

        public async Task<DataX.CalorimetricData?> GetCalorimetricDataAsync(
            [Parent] Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<CalorimetricDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.CalorimetricData;
        }

        private sealed class PhotovoltaicDataData
        {
            public DataX.PhotovoltaicData PhotovoltaicData { get; set; } = default!;
        }

        public async Task<DataX.PhotovoltaicData?> GetPhotovoltaicDataAsync(
            [Parent] Data.Database database,
            Guid id,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return (await QueryDatabase<PhotovoltaicDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.PhotovoltaicData;
        }

        private sealed class AllDataData
        {
            public DataX.DataConnection AllData { get; set; } = default!;
        }

        public async Task<DataX.DataConnection?> GetAllDataAsync(
            [Parent] Data.Database database,
            DataX.DataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            uint? first,
            string? after,
            uint? last,
            string? before,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            var dataConnection = (await QueryDatabase<AllDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        database.Locator.AbsoluteUri == "https://igsdb-icon.herokuapp.com/icon_graphql/"
                        ? new[] {
                              "DataFields.graphql",
                              "AllDataX.graphql"
                          }
                        : new[] {
                              "DataFields.graphql",
                              "PageInfoFields.graphql",
                              "AllData.graphql"
                          }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where = RewriteDataPropositionInput(where, database),
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllData;
            return dataConnection is null ? null : new DataX.DataConnection(
                dataConnection.Edges,
                dataConnection.Nodes,
                dataConnection.TotalCount,
                dataConnection.Timestamp
            );
        }

        private static DataX.DataPropositionInput? RewriteDataPropositionInput(
            DataX.DataPropositionInput? where,
            Data.Database database
            )
        {
            return database.Locator.AbsoluteUri == "https://igsdb-icon.herokuapp.com/icon_graphql/"
                ? (where ?? new DataX.DataPropositionInput(null, null, null, null, null, null, null, null, null, null, null, null, null, null))
                : where;
        }

        private sealed class AllOpticalDataData
        {
            public DataX.OpticalDataConnection AllOpticalData { get; set; } = default!;
        }

        public async Task<DataX.OpticalDataConnection?> GetAllOpticalDataAsync(
            [Parent] Data.Database database,
            DataX.OpticalDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            uint? first,
            string? after,
            uint? last,
            string? before,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<AllOpticalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        database.Locator.AbsoluteUri == "https://igsdb-icon.herokuapp.com/icon_graphql/"
                        ? new[] {
                              "DataFields.graphql",
                              "OpticalDataFields.graphql",
                              "AllOpticalDataX.graphql"
                          }
                        : new[] {
                              "DataFields.graphql",
                              "OpticalDataFields.graphql",
                              "PageInfoFields.graphql",
                              "AllOpticalData.graphql"
                          }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where = RewriteOpticalDataPropositionInput(where, database),
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllOpticalData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllOpticalData;
        }

        private static DataX.OpticalDataPropositionInput? RewriteOpticalDataPropositionInput(
            DataX.OpticalDataPropositionInput? where,
            Data.Database database
            )
        {
            return database.Locator.AbsoluteUri == "https://igsdb-icon.herokuapp.com/icon_graphql/"
                ? (where ?? new DataX.OpticalDataPropositionInput(null, null, null, null, null, null, null, null, null, null, null, null))
                : where;
        }

        private sealed class AllHygrothermalDataData
        {
            public DataX.HygrothermalDataConnection AllHygrothermalData { get; set; } = default!;
        }

        public async Task<DataX.HygrothermalDataConnection?> GetAllHygrothermalDataAsync(
            [Parent] Data.Database database,
            DataX.HygrothermalDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            uint? first,
            string? after,
            uint? last,
            string? before,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<AllHygrothermalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllHygrothermalData;
        }

        private sealed class AllCalorimetricDataData
        {
            public DataX.CalorimetricDataConnection AllCalorimetricData { get; set; } = default!;
        }

        public async Task<DataX.CalorimetricDataConnection?> GetAllCalorimetricDataAsync(
            [Parent] Data.Database database,
            DataX.CalorimetricDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            uint? first,
            string? after,
            uint? last,
            string? before,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<AllCalorimetricDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllCalorimetricData;
        }

        private sealed class AllPhotovoltaicDataData
        {
            public DataX.PhotovoltaicDataConnection AllPhotovoltaicData { get; set; } = default!;
        }

        public async Task<DataX.PhotovoltaicDataConnection?> GetAllPhotovoltaicDataAsync(
            [Parent] Data.Database database,
            DataX.PhotovoltaicDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            uint? first,
            string? after,
            uint? last,
            string? before,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<AllPhotovoltaicDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
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
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllPhotovoltaicData;
        }

        private sealed class HasDataData
        {
            public bool HasData { get; set; }
        }

        public async Task<bool?> GetHasDataAsync(
            [Parent] Data.Database database,
            DataX.DataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<HasDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        new[] {
                            "HasData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where = RewriteDataPropositionInput(where, database),
                        timestamp,
                        locale
                    },
                    operationName: "HasData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.HasData;
        }

        private sealed class HasOpticalDataData
        {
            public bool HasOpticalData { get; set; }
        }

        public async Task<bool?> GetHasOpticalDataAsync(
            [Parent] Data.Database database,
            DataX.OpticalDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<HasOpticalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        new[] {
                            "HasOpticalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where = RewriteOpticalDataPropositionInput(where, database),
                        timestamp,
                        locale
                    },
                    operationName: "HasOpticalData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.HasOpticalData;
        }

        private sealed class HasCalorimetricDataData
        {
            public bool HasCalorimetricData { get; set; }
        }

        public async Task<bool?> GetHasCalorimetricDataAsync(
            [Parent] Data.Database database,
            DataX.CalorimetricDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<HasCalorimetricDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        new[] {
                            "HasCalorimetricData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where,
                        timestamp,
                        locale
                    },
                    operationName: "HasCalorimetricData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.HasCalorimetricData;
        }

        private sealed class HasHygrothermalDataData
        {
            public bool HasHygrothermalData { get; set; }
        }

        public async Task<bool?> GetHasHygrothermalDataAsync(
            [Parent] Data.Database database,
            DataX.HygrothermalDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<HasHygrothermalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        new[] {
                            "HasHygrothermalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where,
                        timestamp,
                        locale
                    },
                    operationName: "HasHygrothermalData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.HasHygrothermalData;
        }

        private sealed class HasPhotovoltaicDataData
        {
            public bool HasPhotovoltaicData { get; set; }
        }

        public async Task<bool?> GetHasPhotovoltaicDataAsync(
            [Parent] Data.Database database,
            DataX.PhotovoltaicDataPropositionInput? where,
            DateTime? timestamp,
            string? locale,
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<HasPhotovoltaicDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await QueryingDatabases.ConstructQuery(
                        new[] {
                            "HasPhotovoltaicData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where,
                        timestamp,
                        locale
                    },
                    operationName: "HasPhotovoltaicData"
                ),
                httpContextAccessor,
                resolverContext,
                cancellationToken
            ).ConfigureAwait(false)
            )?.HasPhotovoltaicData;
        }

        private async
          Task<TGraphQlResponse?>
          QueryDatabase<TGraphQlResponse>(
            Data.Database database,
            GraphQL.GraphQLRequest request,
            IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
            )
          where TGraphQlResponse : class
        {
            try
            {
                var deserializedGraphQlResponse =
                    await QueryingDatabases.QueryDatabase<TGraphQlResponse>(
                        database,
                        request,
                        _httpClientFactory,
                        httpContextAccessor,
                        cancellationToken
                    );
                if (deserializedGraphQlResponse.Errors?.Length >= 1)
                {
                    _logger.LogWarning("Failed with errors {Errors} to query the database {Locator} for {Request}", JsonSerializer.Serialize(deserializedGraphQlResponse.Errors), database.Locator, JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
                    foreach (var error in deserializedGraphQlResponse.Errors)
                    {
                        var errorBuilder = ErrorBuilder.New()
                            .SetCode("DATABASE_QUERY_ERROR")
                            .SetMessage($"The GraphQL response received from the database {database.Locator} for the request {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)} reported the error {error.Message}.")
                            .SetPath(error.Path);
                        if (error.Extensions is not null)
                        {
                            foreach (var (key, value) in error.Extensions)
                            {
                                errorBuilder.SetExtension(key, value);
                            }
                        }
                        // TODO Add `error.Locations` to `errorBuilder`.
                        resolverContext.ReportError(errorBuilder.Build());
                    }
                }
                return deserializedGraphQlResponse.Data;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Failed with status code {StatusCode} to request {Locator} for {Request}.", e.StatusCode, database.Locator, JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
                resolverContext.ReportError(
                    ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage($"Failed with status code {e.StatusCode} to request {database.Locator} for {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)}.")
                    .SetException(e)
                    .Build()
                );
                return null;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Failed to deserialize GraphQL response of request to {Locator} for {Request}. The details given are: Zero-based number of bytes read within the current line before the exception are {BytePositionInLine}, zero-based number of lines read before the exception are {LineNumber}, message that describes the current exception is '{Message}', path within the JSON where the exception was encountered is {Path}.", database.Locator, JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions), e.BytePositionInLine, e.LineNumber, e.Message, e.Path);
                resolverContext.ReportError(
                    ErrorBuilder.New()
                    .SetCode("DESERIALIZATION_FAILED")
                    .SetPath(resolverContext.Path.ToList().Concat(e.Path?.Split('.') ?? Array.Empty<string>()).ToList()) // TODO Splitting the path at '.' is wrong in general.
                    .SetMessage($"Failed to deserialize GraphQL response of request to {database.Locator} for {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)}. The details given are: Zero-based number of bytes read within the current line before the exception are {e.BytePositionInLine}, zero-based number of lines read before the exception are {e.LineNumber}, message that describes the current exception is '{e.Message}', path within the JSON where the exception was encountered is {e.Path}.")
                    .SetException(e)
                    .Build()
                );
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to request {Locator} for {Request} or failed to deserialize the response.", database.Locator, JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
                resolverContext.ReportError(
                    ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage($"Failed to request {database.Locator} for {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)} or failed to deserialize the response.")
                    .SetException(e)
                    .Build()
                );
                return null;
            }
        }
    }
}