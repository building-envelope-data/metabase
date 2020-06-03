using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Marten.Linq.MatchesSql;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using Events = Icon.Events;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;

namespace Icon.Handlers
{
    public abstract class GetOneToManyAssociatesOfModelsHandler<TModel, TAssociateModel, TAggregate, TAssociateAggregate, TCreatedEvent>
      : IQueryHandler<Queries.GetOneToManyAssociatesOfModels<TModel, TAssociateModel>, IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TCreatedEvent : Events.ICreatedEvent
    {
        public static Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedModelIds,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>>> queryAssociateIds,
            CancellationToken cancellationToken
            )
        {
            return GetAssociationsOrAssociatesOfModels<TModel, TAssociateModel, TAggregate, TAssociateAggregate>.Do(
                session,
                timestampedModelIds,
                queryAssociateIds,
                cancellationToken
                );
        }

        private readonly IAggregateRepository _repository;
        private readonly Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>> _queryAssociationIds;

        public GetOneToManyAssociatesOfModelsHandler(
            IAggregateRepository repository,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.Id>, CancellationToken, Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associationId)>>> queryAssociationIds
            )
        {
            _repository = repository;
            _queryAssociationIds = queryAssociationIds;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetOneToManyAssociatesOfModels<TModel, TAssociateModel> query,
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

    /* public class GetOneToManyAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateAggregate, TCreatedEvent> */
    /*   : GetAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateModel, TAssociateAggregate> */
    /*   where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new() */
    /*   where TCreatedEvent : Events.ICreatedEvent */
    /* { */
    /*     public struct Select */
    /*     { */
    /*         public Guid ModelId; */
    /*         public Guid AssociateId; */
    /*     } */

    /*     private readonly Func<Guid[], Expression<Func<TCreatedEvent, bool>>> _where; */
    /*     private readonly Expression<Func<TCreatedEvent, Select>> _select; */

    /*     public GetOneToManyAssociatesOfModelsHandler( */
    /*                     Func<Guid[], Expression<Func<TCreatedEvent, bool>>> where, */
    /*                     Expression<Func<TCreatedEvent, Select>> select, */
    /*                     IAggregateRepository repository */
    /*                     ) */
    /*       : base(repository) */
    /*     { */
    /*         _where = where; */
    /*         _select = select; */
    /*     } */

    /*     protected override async Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>> QueryAssociateIds( */
    /*         IAggregateRepositoryReadOnlySession session, */
    /*         IEnumerable<ValueObjects.Id> modelIds, */
    /*         CancellationToken cancellationToken */
    /*         ) */
    /*     { */
    /*         var modelGuids = modelIds.Select(modelId => (Guid)modelId).ToArray(); */
    /*         return */
    /*           (await session.QueryEvents<TCreatedEvent>() */
    /*             .Where(_where(modelGuids)) */
    /*             .Select(_select) */
    /*             .ToListAsync(cancellationToken) */
    /*             .ConfigureAwait(false) */
    /*             ) */
    /*           .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId)); */
    /*     } */
    /* } */
}