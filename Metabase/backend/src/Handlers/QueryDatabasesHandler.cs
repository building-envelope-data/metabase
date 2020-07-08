using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using GraphQL.Client.Http; // AsGraphQLHttpResponse
using Infrastructure.Aggregates;
using Infrastructure.Queries;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using GraphQLRequest = GraphQL.GraphQLRequest;

namespace Metabase.Handlers
{
    public abstract class QueryDatabasesHandler<TQuery, TResponse, TGraphQlResponse>
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        private readonly IAggregateRepository _repository;

        protected QueryDatabasesHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResponse> Handle(
            TQuery query,
            CancellationToken cancellationToken
            )
        {
            IEnumerable<Models.Database> databases;
            using (var session = _repository.OpenReadOnlySession())
            {
                databases =
                  await LoadDatabases(session, cancellationToken)
                  .ConfigureAwait(false);
            }
            var responseResults =
              await Task.WhenAll(
                  databases.Select(database =>
                    QueryDatabase(database, query, cancellationToken)
                    )
                  )
              .ConfigureAwait(false);
            return MergeDatabaseResponses(query, responseResults);
        }

        private async
          Task<IReadOnlyCollection<Models.Database>>
          LoadDatabases(
              IAggregateRepositoryReadOnlySession session,
              CancellationToken cancellationToken
              )
        {
            return
              (
               await session.GetModels<Models.Database, Aggregates.DatabaseAggregate, Events.DatabaseCreated>(cancellationToken)
               .ConfigureAwait(false)
               )
              .Where(databaseResult => databaseResult.IsSuccess)
              .Select(databaseResult => databaseResult.Value)
              .ToList().AsReadOnly();
        }

        private async
          Task<Result<TResponse, Errors>>
          QueryDatabase(
            Models.Database database,
            TQuery query,
            CancellationToken cancellationToken
            )
        {
            // https://github.com/graphql-dotnet/graphql-client/blob/47b4abfbfda507a91b5c62a18a9789bd3a8079c7/src/GraphQL.Client/GraphQLHttpResponse.cs
            var response =
              (
               await CreateGraphQlClient(database)
               .SendQueryAsync<TGraphQlResponse>(
                 CreateGraphQlRequest(query),
                 cancellationToken
                 )
               .ConfigureAwait(false)
               )
              .AsGraphQLHttpResponse<TGraphQlResponse>();
            if (
                response.StatusCode != System.Net.HttpStatusCode.OK ||
                (response.Errors != null && response.Errors.Length >= 1)
                )
            {
                // TODO Convert all `response.Errors` error to our `Errors` with proper path information!
                return Result.Failure<TResponse, Errors>(
                    Errors.One(
                      message: $"Accessing the database {database} failed with status code {response.StatusCode} and errors {string.Join(", ", response.Errors.Select(GraphQlErrorToString))}",
                      code: ErrorCodes.GraphQlRequestFailed
                      // TODO path: ...
                      )
                    );
            }
            return Result.Ok<TResponse, Errors>(
                  ParseGraphQlResponse(
                    database,
                    response.Data
                    )
                  );
        }

        private string GraphQlErrorToString(GraphQL.GraphQLError error)
        {
            return $"GraphQlError(message: {error.Message})"; // , locations: {string.Join(", ", error.Locations?.Select(GraphQlLocationToString))}, path: {string.Join(", ", error.Path)}, extensions: {string.Join(", ", error.Extensions)}
        }

        private string GraphQlLocationToString(GraphQL.GraphQLLocation location)
        {
            return $"{location.Line}:{location.Column}";
        }

        private GraphQL.Client.Http.GraphQLHttpClient CreateGraphQlClient(
            Models.Database database
            )
        {
            return new GraphQL.Client.Http.GraphQLHttpClient(
                endPoint: database.Locator,
                serializer: new GraphQL.Client.Serializer.SystemTextJson.SystemTextJsonSerializer()
                );
        }

        protected abstract GraphQLRequest CreateGraphQlRequest(
            TQuery query
            );

        protected abstract TResponse ParseGraphQlResponse(
            Models.Database database,
            TGraphQlResponse response
            );

        protected abstract TResponse MergeDatabaseResponses(
            TQuery query,
            IEnumerable<Result<TResponse, Errors>> responseResults
            );
    }
}