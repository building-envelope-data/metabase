using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using Exception = System.Exception;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;

namespace Icon.Handlers
{
    public sealed class GetMethodDevelopersHandler
      : IQueryHandler<Queries.GetModelsForTimestampedIds<Models.MethodDeveloper>, IEnumerable<Result<Models.MethodDeveloper, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate> _getInstitutionMethodDevelopersHandler;
        private readonly GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.PersonMethodDeveloperAggregate> _getPersonMethodDevelopersHandler;

        public GetMethodDevelopersHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _getInstitutionMethodDevelopersHandler = new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>(repository);
            _getPersonMethodDevelopersHandler = new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>(repository);
        }

        public async Task<IEnumerable<Result<Models.MethodDeveloper, Errors>>> Handle(
            Queries.GetModelsForTimestampedIds<Models.MethodDeveloper> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await session.LoadAllX(
                    query.TimestampedIds,
                    (session, aggregateType, timestampedIds, cancellationToken) =>
                    {
                        if (aggregateType == typeof(Aggregates.InstitutionMethodDeveloperAggregate))
                        {
                            return _getInstitutionMethodDevelopersHandler.Handle(
                              session,
                              timestampedIds,
                              cancellationToken
                              );
                        }
                        if (aggregateType == typeof(Aggregates.PersonMethodDeveloperAggregate))
                        {
                            return _getPersonMethodDevelopersHandler.Handle(
                              session,
                              timestampedIds,
                              cancellationToken
                              );
                        }
                        // TODO Return failure result instead.
                        throw new Exception($"The aggregate type {aggregateType} is not supported");
                        /* return Result.Failure<Models.MethodDeveloper, Errors>( */
                        /*     Errors.One( */
                        /*       message: $"The aggregate type {aggregateType} is invalid", */
                        /*       code: ErrorCodes.InvalidType */
                        /*       ) */
                        /*     ); */
                    },
                    cancellationToken
                    )
                    .ConfigureAwait(false);
            }
        }
    }
}