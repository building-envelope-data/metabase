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
using IEventSourcedAggregate = Icon.Infrastructure.Aggregate.IEventSourcedAggregate;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Type = System.Type;

namespace Icon.Handlers
{
    public sealed class GetModelsOfUnknownTypeForTimestampedIdsHandler
      : IQueryHandler<Queries.GetModelsForTimestampedIds<Models.IModel>, IEnumerable<Result<Models.IModel, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly IDictionary<Type, IGetModelsForTimestampedIdsHandler> _aggregateTypeToGetHandler;

        public GetModelsOfUnknownTypeForTimestampedIdsHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _aggregateTypeToGetHandler = new Dictionary<Type, IGetModelsForTimestampedIdsHandler>();
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentAggregate),
                new GetModelsForTimestampedIdsHandler<Models.Component, Aggregates.ComponentAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentConcretizationAggregate),
                new GetModelsForTimestampedIdsHandler<Models.ComponentConcretization, Aggregates.ComponentConcretizationAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentManufacturerAggregate),
                new GetModelsForTimestampedIdsHandler<Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentPartAggregate),
                new GetModelsForTimestampedIdsHandler<Models.ComponentPart, Aggregates.ComponentPartAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentVariantAggregate),
                new GetModelsForTimestampedIdsHandler<Models.ComponentVariant, Aggregates.ComponentVariantAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentVersionAggregate),
                new GetModelsForTimestampedIdsHandler<Models.ComponentVersion, Aggregates.ComponentVersionAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.DatabaseAggregate),
                new GetModelsForTimestampedIdsHandler<Models.Database, Aggregates.DatabaseAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.InstitutionAggregate),
                new GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.InstitutionMethodDeveloperAggregate),
                new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.InstitutionRepresentativeAggregate),
                new GetModelsForTimestampedIdsHandler<Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.MethodAggregate),
                new GetModelsForTimestampedIdsHandler<Models.Method, Aggregates.MethodAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.PersonAffiliationAggregate),
                new GetModelsForTimestampedIdsHandler<Models.PersonAffiliation, Aggregates.PersonAffiliationAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.PersonAggregate),
                new GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.PersonMethodDeveloperAggregate),
                new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.StandardAggregate),
                new GetModelsForTimestampedIdsHandler<Models.Standard, Aggregates.StandardAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.UserAggregate),
                new GetModelsForTimestampedIdsHandler<Models.User, Aggregates.UserAggregate>(repository)
                );
        }

        public async Task<IEnumerable<Result<Models.IModel, Errors>>> Handle(
            Queries.GetModelsForTimestampedIds<Models.IModel> query,
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