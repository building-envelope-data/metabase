using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregates;
using Icon.Infrastructure.Models;
using Icon.Infrastructure.Queries;
using CancellationToken = System.Threading.CancellationToken;
using Exception = System.Exception;
using Type = System.Type;

namespace Icon.Handlers
{
    public sealed class GetModelsOfUnknownTypeForTimestampedIdsHandler
      : IQueryHandler<Queries.GetModelsForTimestampedIds<IModel>, IEnumerable<Result<IModel, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly IDictionary<Type, IGetModelsForTimestampedIdsHandler> _aggregateTypeToGetHandler;

        public GetModelsOfUnknownTypeForTimestampedIdsHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _aggregateTypeToGetHandler = new Dictionary<Type, IGetModelsForTimestampedIdsHandler>
            {
                {
                    typeof(Aggregates.ComponentAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.Component, Aggregates.ComponentAggregate>(repository)
                },
                {
                    typeof(Aggregates.ComponentConcretizationAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.ComponentConcretization, Aggregates.ComponentConcretizationAggregate>(repository)
                },
                {
                    typeof(Aggregates.ComponentManufacturerAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate>(repository)
                },
                {
                    typeof(Aggregates.ComponentPartAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.ComponentPart, Aggregates.ComponentPartAggregate>(repository)
                },
                {
                    typeof(Aggregates.ComponentVariantAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.ComponentVariant, Aggregates.ComponentVariantAggregate>(repository)
                },
                {
                    typeof(Aggregates.ComponentVersionAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.ComponentVersion, Aggregates.ComponentVersionAggregate>(repository)
                },
                {
                    typeof(Aggregates.DatabaseAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.Database, Aggregates.DatabaseAggregate>(repository)
                },
                {
                    typeof(Aggregates.InstitutionAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>(repository)
                },
                {
                    typeof(Aggregates.InstitutionMethodDeveloperAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>(repository)
                },
                {
                    typeof(Aggregates.InstitutionRepresentativeAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>(repository)
                },
                {
                    typeof(Aggregates.MethodAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.Method, Aggregates.MethodAggregate>(repository)
                },
                {
                    typeof(Aggregates.PersonAffiliationAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.PersonAffiliation, Aggregates.PersonAffiliationAggregate>(repository)
                },
                {
                    typeof(Aggregates.PersonAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>(repository)
                },
                {
                    typeof(Aggregates.PersonMethodDeveloperAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>(repository)
                },
                {
                    typeof(Aggregates.StandardAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.Standard, Aggregates.StandardAggregate>(repository)
                },
                {
                    typeof(Aggregates.UserAggregate),
                    new GetModelsForTimestampedIdsHandler<Models.User, Aggregates.UserAggregate>(repository)
                }
            };
        }

        public async Task<IEnumerable<Result<IModel, Errors>>> Handle(
            Queries.GetModelsForTimestampedIds<IModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await session.LoadAllX(
                    query.TimestampedIds,
                    (session, aggregateType, timestampedIds, cancellationToken) =>
                    {
                        if (_aggregateTypeToGetHandler.ContainsKey(aggregateType))
                            return _aggregateTypeToGetHandler[aggregateType].HandleX(
                                session,
                                timestampedIds,
                                cancellationToken
                              );
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