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
using System.Linq.Expressions;

namespace Icon.Handlers
{
    public abstract class GetOneToManyAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateAggregate, TCreatedEvent>
      : GetAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateModel, TAssociateAggregate>
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TCreatedEvent : Events.ICreatedEvent
    {
        public GetOneToManyAssociatesOfModelsHandler(IAggregateRepository repository)
          : base(repository)
        {
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
    /*           (await session.Query<TCreatedEvent>() */
    /*             .Where(_where(modelGuids)) */
    /*             .Select(_select) */
    /*             .ToListAsync(cancellationToken) */
    /*             .ConfigureAwait(false) */
    /*             ) */
    /*           .Select(a => ((ValueObjects.Id)a.ModelId, (ValueObjects.Id)a.AssociateId)); */
    /*     } */
    /* } */
}