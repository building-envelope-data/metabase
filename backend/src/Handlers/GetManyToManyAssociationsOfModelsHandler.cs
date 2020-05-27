using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;
using System.Linq;
using Marten;
using Marten.Linq.MatchesSql;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System;

namespace Icon.Handlers
{
    public abstract class GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      : IQueryHandler<Queries.GetManyToManyAssociationsOfModels<TModel, TAssociationModel>, IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    {
        public static Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>> queryAssociationIds,
            CancellationToken cancellationToken
            )
        {
            return GetAssociationsOrAssociatesOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate>.Do(
                session,
                timestampedModelIds,
                queryAssociationIds,
                cancellationToken
                );
        }

        private readonly IAggregateRepository _repository;
        private readonly Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>> _queryAssociationIds;

        public GetManyToManyAssociationsOfModelsHandler(
            IAggregateRepository repository,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>> queryAssociationIds
            )
        {
            _repository = repository;
            _queryAssociationIds = queryAssociationIds;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>> Handle(
            Queries.GetManyToManyAssociationsOfModels<TModel, TAssociationModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Do(
                    session,
                    query.TimestampedIds,
                    _queryAssociationIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}