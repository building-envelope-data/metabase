using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class RemoveManyToManyAssociationsOfModel
    {
        public static Task<Result<bool, Errors>> Forward<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IAggregateRepositorySession session,
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return Do<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                session,
                timestampedId,
                newAssociationRemovedEvent,
                Handlers.GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>.Do,
                cancellationToken
                );
        }

        public static Task<Result<bool, Errors>> Backward<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IAggregateRepositorySession session,
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return Do<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                session,
                timestampedId,
                newAssociationRemovedEvent,
                Handlers.GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>.Do,
                cancellationToken
                );
        }

        private static async Task<Result<bool, Errors>> Do<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            IAggregateRepositorySession session,
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getManyToManyAssociationsOfModels,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return await (
                await getManyToManyAssociationsOfModels(
                  session,
                  new ValueObjects.TimestampedId[] { timestampedId },
                  cancellationToken
                  )
                  .ConfigureAwait(false)
                )
              .First()
              .Bind(async associationResults =>
                  await associationResults.Combine<TAssociationModel, Errors>()
                  .Bind(async associations =>
                    await session.Delete<TAssociationAggregate>(
                      associations.Select(association => (
                         (ValueObjects.TimestampedId)(association.Id, association.Timestamp), // TODO Casting to `TimestampedId` could result in a run-time error and must not be done!
                         (Events.IEvent)newAssociationRemovedEvent(association.Id)
                         )
                        ),
                      cancellationToken
                      )
                  .ConfigureAwait(false)
                    )
                  .ConfigureAwait(false)
                  )
                  .ConfigureAwait(false);
        }
    }
}