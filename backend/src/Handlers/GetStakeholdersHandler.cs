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
    public sealed class GetStakeholdersHandler
      : IQueryHandler<Queries.GetModelsForTimestampedIds<Models.Stakeholder>, IEnumerable<Result<Models.Stakeholder, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate> _getInstitutionsHandler;
        private readonly GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate> _getPersonsHandler;

        public GetStakeholdersHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _getInstitutionsHandler = new GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>(repository);
            _getPersonsHandler = new GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>(repository);
        }

        public async Task<IEnumerable<Result<Models.Stakeholder, Errors>>> Handle(
            Queries.GetModelsForTimestampedIds<Models.Stakeholder> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await session.LoadAllX(
                    query.TimestampedIds,
                    async (session, aggregateType, timestampedIds, cancellationToken) =>
                    {
                        if (aggregateType == typeof(Aggregates.InstitutionAggregate))
                        {
                            return
                            (await _getInstitutionsHandler.Handle(
                              timestampedIds,
                              session,
                              cancellationToken
                              )
                                .ConfigureAwait(false)
                                )
                            .Select(r =>
                                r.Map(i => (Models.Stakeholder)i)
                                );
                        }
                        if (aggregateType == typeof(Aggregates.PersonAggregate))
                        {
                            return
                              (await _getPersonsHandler.Handle(
                              timestampedIds,
                              session,
                              cancellationToken
                              )
                                .ConfigureAwait(false)
                                ).Select(r =>
                                                                        r.Map(i => (Models.Stakeholder)i)
                                                                        );
                        }
                        // TODO Return failure result instead.
                        throw new Exception($"The aggregate type {aggregateType} is not supported");
                        /* return Result.Failure<Models.Stakeholder, Errors>( */
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