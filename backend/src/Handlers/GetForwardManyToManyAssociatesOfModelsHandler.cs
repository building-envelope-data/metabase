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
    public sealed class GetForwardManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>
      : GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      where TAssociationModel : Models.IManyToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAddedEvent : Events.IAddedEvent
    {
        public GetForwardManyToManyAssociatesOfModelsHandler(IAggregateRepository repository)
          : base(
              repository,
              new GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAddedEvent>(repository)
              )
        {
        }

        protected override ValueObjects.Id GetAssociateId(TAssociationModel association)
        {
            return association.AssociateId;
        }
    }
}