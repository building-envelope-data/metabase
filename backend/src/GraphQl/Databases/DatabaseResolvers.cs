using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GraphQL.Client.Serializer.SystemTextJson;
using System;
using IdentityModel.Client;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using HotChocolate;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Metabase.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Http;
using static IdentityModel.OidcConstants;

namespace Metabase.GraphQl.Databases
{
    public class DatabaseResolvers
    {
        private static readonly JsonSerializerOptions NonDataSerializerOptions =
            new()
            {
                Converters = { new JsonStringEnumConverter(new ConstantCaseJsonNamingPolicy(), false), },
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Disallow,
                IncludeFields = false,
                IgnoreReadOnlyProperties = false,
                IgnoreReadOnlyFields = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }; //.SetupImmutableConverter();

        private static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                Converters = {
                        new JsonStringEnumConverter(new ConstantCaseJsonNamingPolicy(), false),
                        new DataConverterWithTypeDiscriminatorProperty(NonDataSerializerOptions)
                        },
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Disallow,
                IncludeFields = false,
                IgnoreReadOnlyProperties = false,
                IgnoreReadOnlyFields = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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

        public Task<bool> GetCanCurrentUserUpdateNodeAsync(
          [Parent] Data.Institution database,
          [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
          [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
          Data.ApplicationDbContext context,
          CancellationToken cancellationToken
        )
        {
            return DatabaseAuthorization.IsAuthorizedToUpdate(claimsPrincipal, database.Id, userManager, context, cancellationToken);
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

        // Inspired by https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#support-polymorphic-deserialization
        // and https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#an-alternative-way-to-do-polymorphic-deserialization
        public class DataConverterWithTypeDiscriminatorProperty : JsonConverter<DataX.IData>
        {
            private const string DISCRIMINATOR_PROPERTY_NAME = "__typename";

            // type discriminators
            private const string CALORIMETRIC_DATA = "CalorimetricData";
            private const string HYGROTHERMAL_DATA = "HygrothermalData";
            private const string OPTICAL_DATA = "OpticalData";
            private const string PHOTOVOLTAIC_DATA = "PhotovoltaicData";

            private readonly JsonSerializerOptions _options;

            public DataConverterWithTypeDiscriminatorProperty(
                JsonSerializerOptions options
            )
            {
                _options = options;
            }

            public override bool CanConvert(Type typeToConvert) =>
                // typeof(DataX.IData).IsAssignableFrom(typeToConvert);
                typeof(DataX.IData).IsEquivalentTo(typeToConvert);

            public override DataX.IData Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var readerClone = reader; // clones `reader` because it is a struct!
                if (readerClone.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException($"Token is not a start object but {readerClone.TokenType}.");
                }
                readerClone.Read();
                if (readerClone.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Token is not a property but {readerClone.TokenType}.");
                }
                var propertyName = readerClone.GetString();
                if (propertyName != DISCRIMINATOR_PROPERTY_NAME)
                {
                    throw new JsonException($"Property is not discriminator property but {propertyName}.");
                }
                readerClone.Read();
                if (readerClone.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Token is not a string but {readerClone.TokenType}.");
                }
                var typeDiscriminator = readerClone.GetString();
                // Note that you can't pass in the original options instance
                // that registers the converter to `Deserialize` as told on
                // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#an-alternative-way-to-do-polymorphic-deserialization
                return typeDiscriminator switch
                {
                    CALORIMETRIC_DATA => JsonSerializer.Deserialize<DataX.CalorimetricData>(ref reader, _options) ?? throw new JsonException("Could not deserialize calorimetric data."),
                    HYGROTHERMAL_DATA => JsonSerializer.Deserialize<DataX.HygrothermalData>(ref reader, _options) ?? throw new JsonException("Could not deserialize hygrothermal data."),
                    OPTICAL_DATA => JsonSerializer.Deserialize<DataX.OpticalData>(ref reader, _options) ?? throw new JsonException("Could not deserialize optical data."),
                    PHOTOVOLTAIC_DATA => JsonSerializer.Deserialize<DataX.PhotovoltaicData>(ref reader, _options) ?? throw new JsonException("Could not deserialize photovoltaic data."),
                    _ => throw new JsonException($"Type discriminator has unknown value {typeDiscriminator}.")
                };
            }

            public override void Write(
                Utf8JsonWriter writer, DataX.IData data, JsonSerializerOptions options)
            {
                try
                {
                    writer.WriteStartObject();
                    if (data is DataX.CalorimetricData calorimetricData)
                    {
                        writer.WriteString(DISCRIMINATOR_PROPERTY_NAME, CALORIMETRIC_DATA);
                        throw new JsonException("Unsupported!");
                    }
                    else if (data is DataX.HygrothermalData hygrothermalData)
                    {
                        writer.WriteString(DISCRIMINATOR_PROPERTY_NAME, HYGROTHERMAL_DATA);
                        throw new JsonException("Unsupported!");
                    }
                    else if (data is DataX.OpticalData opticalData)
                    {
                        writer.WriteString(DISCRIMINATOR_PROPERTY_NAME, OPTICAL_DATA);
                        throw new JsonException("Unsupported!");
                    }
                    else if (data is DataX.PhotovoltaicData photovoltaicData)
                    {
                        writer.WriteString(DISCRIMINATOR_PROPERTY_NAME, PHOTOVOLTAIC_DATA);
                        throw new JsonException("Unsupported!");
                    }
                    throw new JsonException("Unsupported!");
                }
                finally
                {
                    writer.WriteEndObject();
                }
            }
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
                    query: await ConstructQuery(
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
                    query: await ConstructQuery(
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
                    query: await ConstructQuery(
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
                    query: await ConstructQuery(
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
                    query: await ConstructQuery(
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
                    query: await ConstructQuery(
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
                    query: await ConstructQuery(
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
            [Service] IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
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
                // An alternative to get the bearer token could look something like
                // `httpContextAccessor.HttpContext.GetTokenAsync(AuthenticationSchemes.AuthorizationHeaderBearer)`
                var bearerToken = httpContextAccessor.HttpContext?.Request?.Headers?.Authorization
                    .FirstOrDefault(
                        x => x is not null
                         && x.TrimStart().StartsWith(AuthenticationSchemes.AuthorizationHeaderBearer, StringComparison.Ordinal))
                    ?.Trim()
                    ?.Replace($"{AuthenticationSchemes.AuthorizationHeaderBearer} ", "");
                if (bearerToken is not null)
                {
                    httpClient.SetBearerToken(bearerToken);
                }
                // For some reason `httpClient.PostAsJsonAsync` without `MakeJsonHttpContent` but with `SerializerOptions` results in `BadRequest` status code. It has to do with `JsonContent.Create` used within `PostAsJsonAsync` --- we also cannot use `JsonContent.Create` in `MakeJsonHttpContent`. What is happening here?
                var httpResponseMessage =
                    await httpClient.PostAsync(
                        database.Locator,
                        MakeJsonHttpContent(request),
                        cancellationToken
                    ).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogWarning("Failed with status code {StatusCode} to query the database {Locator} for {Json}.", httpResponseMessage.StatusCode, database.Locator, JsonSerializer.Serialize(request, SerializerOptions));
                    resolverContext.ReportError(
                        ErrorBuilder.New()
                        .SetCode("HTTP_STATUS_CODE_IS_NOT_OK")
                        .SetPath(resolverContext.Path)
                        .SetMessage($"Failed with status code {httpResponseMessage.StatusCode} to query the database {database.Locator} for {JsonSerializer.Serialize(request, SerializerOptions)}.")
                        .Build()
                    );
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
                    _logger.LogWarning("Failed to deserialize the GraphQL response received from the database {Locator} for the request {Request}.", database.Locator, JsonSerializer.Serialize(request, SerializerOptions));
                    resolverContext.ReportError(
                        ErrorBuilder.New()
                        .SetCode("DESERIALIZATION_FAILED")
                        .SetPath(resolverContext.Path)
                        .SetMessage($"Failed to deserialize the GraphQL response received from the database {database.Locator} for the request {JsonSerializer.Serialize(request, SerializerOptions)}.")
                        .Build()
                    );
                    return null;
                }
                if (deserializedGraphQlResponse.Errors?.Length >= 1)
                {
                    _logger.LogWarning("Failed with errors {Errors} to query the database {Locator} for {Request}", JsonSerializer.Serialize(deserializedGraphQlResponse.Errors), database.Locator, JsonSerializer.Serialize(request, SerializerOptions));
                    foreach (var error in deserializedGraphQlResponse.Errors)
                    {
                        var errorBuilder = ErrorBuilder.New()
                            .SetCode("DATABASE_QUERY_ERROR")
                            .SetMessage($"The GraphQL response received from the database {database.Locator} for the request {JsonSerializer.Serialize(request, SerializerOptions)} reported the error {error.Message}.")
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
                _logger.LogError(e, "Failed with status code {StatusCode} to request {Locator} for {Request}.", e.StatusCode, database.Locator, JsonSerializer.Serialize(request, SerializerOptions));
                resolverContext.ReportError(
                    ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage($"Failed with status code {e.StatusCode} to request {database.Locator} for {JsonSerializer.Serialize(request, SerializerOptions)}.")
                    .SetException(e)
                    .Build()
                );
                return null;
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Failed to deserialize GraphQL response of request to {Locator} for {Request}. The details given are: Zero-based number of bytes read within the current line before the exception are {BytePositionInLine}, zero-based number of lines read before the exception are {LineNumber}, message that describes the current exception is '{Message}', path within the JSON where the exception was encountered is {Path}.", database.Locator, JsonSerializer.Serialize(request, SerializerOptions), e.BytePositionInLine, e.LineNumber, e.Message, e.Path);
                resolverContext.ReportError(
                    ErrorBuilder.New()
                    .SetCode("DESERIALIZATION_FAILED")
                    .SetPath(resolverContext.Path.ToList().Concat(e.Path?.Split('.') ?? Array.Empty<string>()).ToList()) // TODO Splitting the path at '.' is wrong in general.
                    .SetMessage($"Failed to deserialize GraphQL response of request to {database.Locator} for {JsonSerializer.Serialize(request, SerializerOptions)}. The details given are: Zero-based number of bytes read within the current line before the exception are {e.BytePositionInLine}, zero-based number of lines read before the exception are {e.LineNumber}, message that describes the current exception is '{e.Message}', path within the JSON where the exception was encountered is {e.Path}.")
                    .SetException(e)
                    .Build()
                );
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to request {Locator} for {Request} or failed to deserialize the response.", database.Locator, JsonSerializer.Serialize(request, SerializerOptions));
                resolverContext.ReportError(
                    ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage($"Failed to request {database.Locator} for {JsonSerializer.Serialize(request, SerializerOptions)} or failed to deserialize the response.")
                    .SetException(e)
                    .Build()
                );
                return null;
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