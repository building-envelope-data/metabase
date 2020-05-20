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
    public sealed class GetBackwardManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAddedEvent>
      : GetManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate>
      where TAssociationModel : Models.IManyToManyAssociation
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAddedEvent : Events.IAddedEvent
    {
        public GetBackwardManyToManyAssociatesOfModelsHandler(IAggregateRepository repository)
          : base(
              repository,
              new GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAddedEvent>(repository)
              )
        {
        }

        protected override ValueObjects.Id GetAssociateId(TAssociationModel association)
        {
            return association.ParentId;
        }
    }
}