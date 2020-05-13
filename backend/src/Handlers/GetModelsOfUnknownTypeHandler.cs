using Guid = System.Guid;
using Exception = System.Exception;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Marten;
using DateTime = System.DateTime;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Aggregates = Icon.Aggregates;
using System.Linq;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using Type = System.Type;
using IEventSourcedAggregate = Icon.Infrastructure.Aggregate.IEventSourcedAggregate;

namespace Icon.Handlers
{
    public sealed class GetModelsOfUnknownTypeHandler
      : IQueryHandler<Queries.GetModels<Models.IModel>, IEnumerable<Result<Models.IModel, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly IDictionary<Type, IGetModelsHandler> _aggregateTypeToGetHandler;

        public GetModelsOfUnknownTypeHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _aggregateTypeToGetHandler = new Dictionary<Type, IGetModelsHandler>();
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentAggregate),
                new GetModelsHandler<Models.Component, Aggregates.ComponentAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentConcretizationAggregate),
                new GetModelsHandler<Models.ComponentConcretization, Aggregates.ComponentConcretizationAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentManufacturerAggregate),
                new GetModelsHandler<Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentPartAggregate),
                new GetModelsHandler<Models.ComponentPart, Aggregates.ComponentPartAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentVariantAggregate),
                new GetModelsHandler<Models.ComponentVariant, Aggregates.ComponentVariantAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.ComponentVersionAggregate),
                new GetModelsHandler<Models.ComponentVersion, Aggregates.ComponentVersionAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.DatabaseAggregate),
                new GetModelsHandler<Models.Database, Aggregates.DatabaseAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.InstitutionAggregate),
                new GetModelsHandler<Models.Institution, Aggregates.InstitutionAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.InstitutionMethodDeveloperAggregate),
                new GetModelsHandler<Models.MethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.InstitutionRepresentativeAggregate),
                new GetModelsHandler<Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.MethodAggregate),
                new GetModelsHandler<Models.Method, Aggregates.MethodAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.PersonAffiliationAggregate),
                new GetModelsHandler<Models.PersonAffiliation, Aggregates.PersonAffiliationAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.PersonAggregate),
                new GetModelsHandler<Models.Person, Aggregates.PersonAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.PersonMethodDeveloperAggregate),
                new GetModelsHandler<Models.MethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.StandardAggregate),
                new GetModelsHandler<Models.Standard, Aggregates.StandardAggregate>(repository)
                );
            _aggregateTypeToGetHandler.Add(
                typeof(Aggregates.UserAggregate),
                new GetModelsHandler<Models.User, Aggregates.UserAggregate>(repository)
                );
        }

        public async Task<IEnumerable<Result<Models.IModel, Errors>>> Handle(
            Queries.GetModels<Models.IModel> query,
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
                                timestampedIds,
                                session,
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