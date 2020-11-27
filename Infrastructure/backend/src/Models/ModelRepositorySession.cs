using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Marten;

namespace Infrastructure.Models
{
    public sealed class ModelRepositorySession
      : ModelRepositoryReadOnlySession
    {
        private readonly Marten.IDocumentSession _session;
        private readonly Events.IEventBus _eventBus;
        private IEnumerable<Events.IEvent> _unsavedEvents;

        public ModelRepositorySession(
            Marten.IDocumentSession session,
            Events.IEventBus eventBus
            )
          : base(session)
        {
            _session = session;
            _eventBus = eventBus;
            _unsavedEvents = Enumerable.Empty<Events.IEvent>();
        }

        // TODO Register checks to perform before completing the transaction on save, that make sure that all models are valid afterwards. For example, by adding `IsApplicable` methods to models, or applying the events to aggregates and converting the new aggregates to models. In both cases, we need to know the model class!
        public async Task<Result<ValueObjects.Id, Errors>> Create<TAggregate>(
            Func<ValueObjects.Id, Events.ICreatedEvent> newCreatedEvent,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            var id = await GenerateNewId(cancellationToken).ConfigureAwait(false);
            var @event = newCreatedEvent(id);
            if (@event.AggregateId != (Guid)id)
            {
                throw new Exception($"The aggregate identifier of the created event {@event.AggregateId} differs from the generated identifier {(Guid)id}");
            }
            return (
              await Create<TAggregate>(
                  @event,
                  cancellationToken
                )
              .ConfigureAwait(false)
              )
              .Map(_ => id);
        }

        public Task<Result<ValueObjects.Id, Errors>> Create<TAggregate>(
            Events.ICreatedEvent @event,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            return
              ValueObjects.Id
              .From(@event.AggregateId)
              .Bind(async id =>
                  (await RegisterEvents(
                    new Events.IEvent[] { @event },
                    eventArray => _session.Events.StartStream<TAggregate>(@event.AggregateId, eventArray),
                    cancellationToken
                    )
                    .ConfigureAwait(false)
                  )
                  .Map(_ => id)
                  );
        }

        public Task<Result<bool, Errors>> Append<TAggregate>(
            ValueObjects.TimestampedId timestampedId,
            Events.IEvent @event,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            return Append<TAggregate>(
                timestampedId,
                new Events.IEvent[] { @event },
                cancellationToken
                );
        }

        public async Task<Result<bool, Errors>> Append<TAggregate>(
            ValueObjects.TimestampedId timestampedId,
            IEnumerable<Events.IEvent> events,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            return await (
                await FetchVersion<TAggregate>(
                  timestampedId,
                  cancellationToken
                  )
               .ConfigureAwait(false)
               )
              .Bind(async version => await
                  RegisterEvents(
                    events,
                    eventArray => _session.Events.Append(
                    timestampedId.Id, version + eventArray.Length, eventArray
                    ),
                    cancellationToken
                    )
                  .ConfigureAwait(false)
                )
              .ConfigureAwait(false);
        }

        public Task<Result<bool, Errors>> Delete<TAggregate>(
            ValueObjects.Timestamp timestamp,
            Events.IDeletedEvent @event,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            ThrowIfDisposed();
            return
              ValueObjects.TimestampedId
              .From(@event.AggregateId, timestamp)
              .Bind(timestampedId =>
                  {
                      _session.Delete<TAggregate>(timestampedId.Id);
                      return Append<TAggregate>(
                          timestampedId,
                          @event,
                          cancellationToken
                          );
                  }
                  );
        }

        public async Task<Result<bool, Errors>> Delete<TAggregate>(
            IEnumerable<(ValueObjects.Timestamp, Events.IDeletedEvent)> timestampsAndEvents,
            CancellationToken cancellationToken
            )
          where TAggregate : class, Aggregates.IAggregate, new()
        {
            var deletionResults = new List<Result<bool, Errors>>();
            foreach (var (timestamp, @event) in timestampsAndEvents)
            {
                deletionResults.Add(
                    await Delete<TAggregate>(
                      timestamp, @event, cancellationToken
                      )
                    .ConfigureAwait(false)
                    );
            }
            return Result.Combine<bool, Errors>(deletionResults);
        }

        public async Task<Result<bool, Errors>> Save(
            CancellationToken cancellationToken
            )
        {
            try
            {
                await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Marten.Services.EventStreamUnexpectedMaxEventIdException exception)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"The aggregate with identifier {exception.Id} of type {exception.AggregateType} was changed after the given timestamp causing the exception {exception}",
                      code: ErrorCodes.OutOfDate
                      )
                );
            }
            catch (Marten.Events.ExistingStreamIdCollisionException exception)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"The identifier {exception.Id} is already in use and can not be used for the new aggregate of type {exception.AggregateType} causing the exception {exception}",
                      code: ErrorCodes.IdCollision
                      )
                );
            }
            await _eventBus.Publish(_unsavedEvents).ConfigureAwait(false);
            _unsavedEvents = Enumerable.Empty<Events.IEvent>();
            return
              await Task.FromResult<Result<bool, Errors>>(
                  Result.Success<bool, Errors>(true)
                  )
              .ConfigureAwait(false);
        }

        private async Task<Result<bool, Errors>> RegisterEvents(
            IEnumerable<Events.IEvent> events,
            Action<Events.IEvent[]> action,
            CancellationToken cancellationToken
            )
        {
            var eventArray = events.ToArray();
            return
              await Errors.CombineX(
                eventArray.Select(@event => (IResult)@event.Validate())
                )
              .Bind(async _ =>
                  await
                  (
                   await CheckExistenceOfCreators(
                     eventArray.Select(@event => @event.CreatorId),
                     cancellationToken
                     )
                   .ConfigureAwait(false)
                  )
                  .Bind(async _ =>
                  {
                      action(eventArray);
                      _unsavedEvents = _unsavedEvents.Concat(eventArray);
                      return
                        await Task.FromResult<Result<bool, Errors>>(
                            Result.Success<bool, Errors>(true)
                            )
                        .ConfigureAwait(false);
                  }
                  )
                   .ConfigureAwait(false)
                  )
              .ConfigureAwait(false);
        }

        private Task<Result<bool, Errors>> CheckExistenceOfCreators(
            IEnumerable<Guid> creatorIds,
            CancellationToken cancellationToken
            )
        {
            return Task.FromResult<Result<bool, Errors>>(
                Result.Success<bool, Errors>(true)
                );
            /* foreach (var creatorId in creatorIds) */
            /* { */
            /*   if (!_session.Query<UserAggregate>().AnyAsync(user => user.Id == creatorId, cancellationToken)) */
            /*     throw new ArgumentException("Creator does not exist!", nameof(creatorId)); */
            /* } */
        }

        //////////////////
        // Associations //
        //////////////////

        public Task<Result<ValueObjects.Id, Errors>> AddManyToManyAssociation<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
            Func<ValueObjects.Id, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAssociationModel : Models.IManyToManyAssociation
          where TAssociateModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
        {
            return AddAssociation<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                newAssociationAddedEvent,
                async @event =>
                {
                    var doesAssociationExist =
                  await QueryAggregates<TAssociationAggregate>()
                  .Where(a =>
                      a.ParentId == @event.ParentId &&
                      a.AssociateId == @event.AssociateId
                      )
                  .AnyAsync(cancellationToken)
                  .ConfigureAwait(false);
                    return doesAssociationExist
                      ? Result.Failure<bool, Errors>(
                          Errors.One(
                            message: $"There already is an association between {@event.ParentId} and {@event.AssociateId} of type {typeof(TAssociationAggregate)}",
                            code: ErrorCodes.AlreadyExistingAssociation
                            )
                          )
                      : Result.Success<bool, Errors>(true);
                },
                addAssociationCheck,
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.Id, Errors>> AddOneToManyAssociation<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
            Func<ValueObjects.Id, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociateModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
        {
            return AddAssociation<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                newAssociationAddedEvent,
                async @event =>
                {
                    var doesAssociationExist =
                  await QueryAggregates<TAssociationAggregate>()
                  .Where(a => a.AssociateId == @event.AssociateId)
                  .AnyAsync(cancellationToken)
                  .ConfigureAwait(false);
                    return doesAssociationExist
                      ? Result.Failure<bool, Errors>(
                          Errors.One(
                            message: $"There already is an association for {@event.AssociateId} of type {typeof(TAssociationAggregate)}",
                            code: ErrorCodes.AlreadyExistingAssociation
                            )
                          )
                      : Result.Success<bool, Errors>(true);
                },
                addAssociationCheck,
                cancellationToken
                );
        }

        private async Task<Result<ValueObjects.Id, Errors>> AddAssociation<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
            Func<ValueObjects.Id, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            Func<Events.IAssociationAddedEvent, Task<Result<bool, Errors>>> failIfAssociationAlreadyExists, // The boolean is ignored. Instead, `Result#IsFailure` is used.
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TModel : Models.IModel
          where TAssociationModel : Models.IAssociation
          where TAssociateModel : Models.IModel
          where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
          where TAssociationAggregate : class, Aggregates.IAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
          where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
        {
            ThrowIfDisposed();
            var id = await GenerateNewId(cancellationToken).ConfigureAwait(false);
            var @event = newAssociationAddedEvent(id);
            if (@event.AggregateId != (Guid)id)
            {
                throw new Exception($"The aggregate identifier of the association added event {@event.AggregateId} differs from the generated identifier {(Guid)id}");
            }
            (Result<TModel, Errors>? parentResult, Result<TAssociateModel, Errors>? associateResult) =
              addAssociationCheck switch
              {
                  AddAssociationCheck.NONE =>
                    (null, null),
                  AddAssociationCheck.PARENT =>
                    (
                      (Result<TModel, Errors>?)(
                        await ValueObjects.Id.From(@event.ParentId)
                        .Bind(async parentId =>
                          await Load<TModel, TAggregate>(
                          parentId,
                          cancellationToken
                          )
                        .ConfigureAwait(false)
                        )
                        .ConfigureAwait(false)
                      ),
                      null
                    ),
                  AddAssociationCheck.ASSOCIATE =>
                    (
                      null,
                      (Result<TAssociateModel, Errors>?)(
                        await ValueObjects.Id.From(@event.AssociateId)
                        .Bind(async associateId =>
                          await Load<TAssociateModel, TAssociateAggregate>(
                          associateId,
                          cancellationToken
                          )
                        .ConfigureAwait(false)
                        )
                        .ConfigureAwait(false)
                      )
                    ),
                  AddAssociationCheck.PARENT_AND_ASSOCIATE =>
                    ((Result<TModel, Errors>?, Result<TAssociateModel, Errors>?))(
                      await Load<TModel, TAggregate, TAssociateModel, TAssociateAggregate>(
                        (
                         ValueObjects.Id.From(@event.ParentId),
                         ValueObjects.Id.From(@event.AssociateId)
                        ),
                        cancellationToken
                        )
                    .ConfigureAwait(false)
                    ),
                  // God-damned C# does not have switch expression exhaustiveness for
                  // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                  _ => throw new Exception($"The check {addAssociationCheck} fell through")
              };
            return
              await Errors.CombineExistent(
                  parentResult,
                  associateResult
                  )
              .Bind(async _ => await
                    (await failIfAssociationAlreadyExists(@event).ConfigureAwait(false))
                    .Bind(async _ =>
                      await Create<TAssociationAggregate>(
                        @event,
                        cancellationToken
                        )
                      .ConfigureAwait(false)
                      )
                    .ConfigureAwait(false)
                  )
              .ConfigureAwait(false);
        }

        public async
          Task<Result<ValueObjects.Id, Errors>>
          RemoveManyToManyAssociation<TAssociationModel, TAssociationAggregate>(
            (ValueObjects.Id from, ValueObjects.Id to) ids,
            ValueObjects.Timestamp timestamp,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IManyToManyAssociation
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
        {
            var maybeAssociationId =
              await QueryAggregates<TAssociationAggregate>()
              .Where(a =>
                  a.ParentId == ids.from &&
                  a.AssociateId == ids.to
                  )
              .Select(a => a.Id)
              .FirstOrDefaultAsync(cancellationToken)
              .ConfigureAwait(false);
            return
              await ValueObjects.Id.From(maybeAssociationId)
              .Bind(async associationId =>
                  (
                   await Delete<TAssociationAggregate>(
                     timestamp,
                     newAssociationRemovedEvent(associationId),
                     cancellationToken
                     )
                   .ConfigureAwait(false)
                  )
                  .Map(_ => associationId)
                  )
              .ConfigureAwait(false);
        }

        public async
          Task<Result<ValueObjects.Id, Errors>>
          RemoveOneToManyAssociation<TAssociationModel, TAssociationAggregate>(
            ValueObjects.Id toId,
            ValueObjects.Timestamp timestamp,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationModel : Models.IOneToManyAssociation
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
        {
            var maybeAssociationId =
              await QueryAggregates<TAssociationAggregate>()
              .Where(a => a.AssociateId == toId)
              .Select(a => a.Id)
              .FirstOrDefaultAsync(cancellationToken)
              .ConfigureAwait(false);
            return
              await ValueObjects.Id.From(maybeAssociationId)
              .Bind(async associationId =>
                  (
                   await Delete<TAssociationAggregate>(
                     timestamp,
                     newAssociationRemovedEvent(associationId),
                     cancellationToken
                     )
                   .ConfigureAwait(false)
                  )
                  .Map(_ => associationId)
                  )
              .ConfigureAwait(false);
        }

        public
          Task<Result<bool, Errors>>
          RemoveForwardManyToManyAssociationsOfModel<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAssociationAggregate, TAssociationAddedEvent>(
                timestampedId,
                newAssociationRemovedEvent,
                GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public
          Task<Result<bool, Errors>>
          RemoveBackwardManyToManyAssociationsOfModel<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAssociationAggregate, TAssociationAddedEvent>(
                timestampedId,
                newAssociationRemovedEvent,
                GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public
          Task<Result<bool, Errors>>
          RemoveForwardOneToManyAssociationsOfModel<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAssociationAggregate, TAssociationAddedEvent>(
                timestampedId,
                newAssociationRemovedEvent,
                GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        public
          Task<Result<bool, Errors>>
          RemoveBackwardOneToManyAssociationOfModel<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAssociateAggregate : class, Aggregates.IAggregate, Aggregates.IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAssociationAggregate, TAssociationAddedEvent>(
                timestampedId,
                newAssociationRemovedEvent,
                GetBackwardOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        private async
          Task<Result<bool, Errors>>
          RemoveAssociationsOfModel<TAssociationModel, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            Func<IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getAssociationsOfModels,
            CancellationToken cancellationToken
            )
            where TAssociationModel : IAssociation
            where TAssociationAggregate : class, Aggregates.IAssociationAggregate, Aggregates.IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return await (
                await getAssociationsOfModels(
                  new ValueObjects.TimestampedId[] { timestampedId },
                  cancellationToken
                  )
                  .ConfigureAwait(false)
                )
              .First()
              .Bind(async associationResults =>
                  await associationResults.Combine<TAssociationModel, Errors>()
                  .Bind(async associations =>
                    await Delete<TAssociationAggregate>(
                      associations.Select(association => (
                         association.Timestamp,
                         (Events.IDeletedEvent)newAssociationRemovedEvent(association.Id)
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