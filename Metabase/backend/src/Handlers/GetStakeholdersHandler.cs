using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Handlers;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using Exception = System.Exception;

namespace Metabase.Handlers
{
    public sealed class GetStakeholdersHandler
      : IQueryHandler<GetModelsForTimestampedIds<Models.Stakeholder>, IEnumerable<Result<Models.Stakeholder, Errors>>>
    {
        private readonly IModelRepository _repository;
        private readonly GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate> _getInstitutionsHandler;
        private readonly GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate> _getPersonsHandler;

        public GetStakeholdersHandler(IModelRepository repository)
        {
            _repository = repository;
            _getInstitutionsHandler = new GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>(repository);
            _getPersonsHandler = new GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>(repository);
        }

        public async Task<IEnumerable<Result<Models.Stakeholder, Errors>>> Handle(
            GetModelsForTimestampedIds<Models.Stakeholder> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await session.LoadAllX<Models.Stakeholder>(
                    query.TimestampedIds,
                    async (session, aggregateType, timestampedIds, cancellationToken) =>
                    {
                        if (aggregateType == typeof(Aggregates.InstitutionAggregate))
                        {
                            return
                            (await _getInstitutionsHandler.Handle(
                              session,
                              timestampedIds,
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
                              session,
                              timestampedIds,
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