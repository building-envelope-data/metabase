// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Icon.Events;
using Marten;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Infrastructure.Aggregate
{
    public sealed class AggregateRepositorySession : AggregateRepositoryReadOnlySession, IAggregateRepositorySession
    {
        private readonly IDocumentSession _session;
        private readonly IEventBus _eventBus;
        private IEnumerable<IEvent> _unsavedEvents;

        public AggregateRepositorySession(IDocumentSession session, IEventBus eventBus)
          : base(session)
        {
            _session = session;
            _eventBus = eventBus;
            _unsavedEvents = Enumerable.Empty<IEvent>();
        }

        public async Task<Result<ValueObjects.Id, Errors>> Create<T>(
            Func<Guid, Events.ICreatedEvent> newCreatedEvent,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var id = await GenerateNewId(cancellationToken).ConfigureAwait(false);
            var @event = newCreatedEvent(id);
            if (@event.AggregateId != (Guid)id)
            {
                throw new Exception($"The aggregate identifier of the created event {@event.AggregateId} differs from the generated identifier {(Guid)id}");
            }
            return (
              await Create<T>(
                  @event,
                  cancellationToken
                )
              .ConfigureAwait(false)
              )
              .Map(_ => id);
        }

        public Task<Result<ValueObjects.Id, Errors>> Create<T>(
            ICreatedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return
              ValueObjects.Id
              .From(@event.AggregateId)
              .Bind(async id =>
                  (await RegisterEvents(
                    new IEvent[] { @event },
                    eventArray => _session.Events.StartStream<T>(@event.AggregateId, eventArray),
                    cancellationToken
                    )
                    .ConfigureAwait(false)
                  )
                  .Map(_ => id)
                  );
        }

        public Task<Result<bool, Errors>> Append<T>(
            ValueObjects.TimestampedId timestampedId,
            IEvent @event,
            CancellationToken cancellationToken
            ) where T : class, IEventSourcedAggregate, new()
        {
            return Append<T>(
                timestampedId,
                new IEvent[] { @event },
                cancellationToken
                );
        }

        public async Task<Result<bool, Errors>> Append<T>(
            ValueObjects.TimestampedId timestampedId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return await (
                await FetchVersion<T>(
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

        public Task<Result<bool, Errors>> Delete<T>(
            ValueObjects.Timestamp timestamp,
            IDeletedEvent @event,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            return
              ValueObjects.TimestampedId
              .From(@event.AggregateId, timestamp)
              .Bind(timestampedId =>
                  {
                      _session.Delete<T>(timestampedId.Id);
                      return Append<T>(
                          timestampedId,
                          @event,
                          cancellationToken
                          );
                  }
                  );
        }

        public async Task<Result<bool, Errors>> Delete<T>(
            IEnumerable<(ValueObjects.Timestamp, IDeletedEvent)> timestampsAndEvents,
            CancellationToken cancellationToken
            )
          where T : class, IEventSourcedAggregate, new()
        {
            var deletionResults = new List<Result<bool, Errors>>();
            foreach (var (timestamp, @event) in timestampsAndEvents)
            {
                deletionResults.Add(
                    await Delete<T>(
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
            _unsavedEvents = Enumerable.Empty<IEvent>();
            return
              await Task.FromResult<Result<bool, Errors>>(
                  Result.Ok<bool, Errors>(true)
                  )
              .ConfigureAwait(false);
        }

        private async Task<Result<bool, Errors>> RegisterEvents(
            IEnumerable<IEvent> events,
            Action<IEvent[]> action,
            CancellationToken cancellationToken
            )
        {
            var eventArray = events.ToArray();
            Event.EnsureValid(eventArray);
            await AssertExistenceOfCreators(
                eventArray.Select(@event => @event.CreatorId),
                cancellationToken
                )
              .ConfigureAwait(false);
            action(eventArray);
            _unsavedEvents = _unsavedEvents.Concat(eventArray);
            return
              await Task.FromResult<Result<bool, Errors>>(
                  Result.Ok<bool, Errors>(true)
                  )
              .ConfigureAwait(false);
        }

        private Task AssertExistenceOfCreators(
            IEnumerable<Guid> creatorIds,
            CancellationToken cancellationToken
            )
        {
            return Task.CompletedTask;
            /* foreach (var creatorId in creatorIds) */
            /* { */
            /*   if (!_session.Query<UserAggregate>().AnyAsync(user => user.Id == creatorId, cancellationToken)) */
            /*     throw new ArgumentException("Creator does not exist!", nameof(creatorId)); */
            /* } */
        }

        //////////////////
        // Associations //
        //////////////////

        public Task<Result<ValueObjects.Id, Errors>> AddManyToManyAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, Aggregates.IManyToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new()
        {
            return AddAssociation<TParent, TAssociation, TAssociate>(
                newAssociationAddedEvent,
                async @event =>
                {
                    var doesAssociationExist =
                  await Query<TAssociation>()
                  .Where(a =>
                      a.ParentId == @event.ParentId &&
                      a.AssociateId == @event.AssociateId
                      )
                  .AnyAsync(cancellationToken)
                  .ConfigureAwait(false);
                    return doesAssociationExist
                      ? Result.Failure<bool, Errors>(
                          Errors.One(
                            message: $"There already is an association between {@event.ParentId} and {@event.AssociateId} of type {typeof(TAssociation)}",
                            code: ErrorCodes.AlreadyExistingAssociation
                            )
                          )
                      : Result.Ok<bool, Errors>(true);
                },
                addAssociationCheck,
                cancellationToken
                );
        }

        public Task<Result<ValueObjects.Id, Errors>> AddOneToManyAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, Aggregates.IOneToManyAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new()
        {
            return AddAssociation<TParent, TAssociation, TAssociate>(
                newAssociationAddedEvent,
                async @event =>
                {
                    var doesAssociationExist =
                  await Query<TAssociation>()
                  .Where(a => a.AssociateId == @event.AssociateId)
                  .AnyAsync(cancellationToken)
                  .ConfigureAwait(false);
                    return doesAssociationExist
                      ? Result.Failure<bool, Errors>(
                          Errors.One(
                            message: $"There already is an association for {@event.AssociateId} of type {typeof(TAssociation)}",
                            code: ErrorCodes.AlreadyExistingAssociation
                            )
                          )
                      : Result.Ok<bool, Errors>(true);
                },
                addAssociationCheck,
                cancellationToken
                );
        }

        private async Task<Result<ValueObjects.Id, Errors>> AddAssociation<TParent, TAssociation, TAssociate>(
            Func<Guid, Events.IAssociationAddedEvent> newAssociationAddedEvent,
            Func<Events.IAssociationAddedEvent, Task<Result<bool, Errors>>> failIfAssociationAlreadyExists, // The boolean is ignored. Instead, `Result#IsFailure` is used.
            AddAssociationCheck addAssociationCheck,
            CancellationToken cancellationToken
            )
          where TParent : class, IEventSourcedAggregate, new()
          where TAssociation : class, Aggregates.IAssociationAggregate, new()
          where TAssociate : class, IEventSourcedAggregate, new()
        {
            AssertNotDisposed();
            var id = await GenerateNewId(cancellationToken).ConfigureAwait(false);
            var @event = newAssociationAddedEvent(id);
            if (@event.AggregateId != (Guid)id)
            {
                throw new Exception($"The aggregate identifier of the association added event {@event.AggregateId} differs from the generated identifier {(Guid)id}");
            }
            // TODO Why are the null-forgiving operators `!` necessary in the expressions `null!` below?
            (Result<TParent, Errors>? parentResult, Result<TAssociate, Errors>? associateResult) =
              addAssociationCheck switch
              {
                  AddAssociationCheck.NONE =>
                    (null!, null!),
                  AddAssociationCheck.PARENT =>
                    (
                     await Load<TParent>(
                       @event.ParentId,
                       cancellationToken
                       )
                     .ConfigureAwait(false),
                     null!
                     ),
                  AddAssociationCheck.ASSOCIATE =>
                    (
                     null!,
                     await Load<TAssociate>(
                       @event.AssociateId,
                       cancellationToken
                       )
                     .ConfigureAwait(false)
                     ),
                  AddAssociationCheck.PARENT_AND_ASSOCIATE =>
                    await Load<TParent, TAssociate>(
                        (
                         @event.ParentId,
                         @event.AssociateId
                        ),
                        cancellationToken
                        )
                    .ConfigureAwait(false),
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
                      await Create<TAssociation>(
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
          RemoveManyToManyAssociation<TAssociationAggregate>(
            (Guid from, Guid to) ids,
            ValueObjects.Timestamp timestamp,
            Func<Guid, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, new()
        {
            var maybeAssociationId =
              await Query<TAssociationAggregate>()
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
          RemoveOneToManyAssociation<TAssociationAggregate>(
            Guid toId,
            ValueObjects.Timestamp timestamp,
            Func<Guid, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            CancellationToken cancellationToken
            )
          where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, new()
        {
            var maybeAssociationId =
              await Query<TAssociationAggregate>()
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
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
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
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
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
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
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
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            return RemoveAssociationsOfModel<TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                timestampedId,
                newAssociationRemovedEvent,
                GetBackwardOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>,
                cancellationToken
                );
        }

        private async
          Task<Result<bool, Errors>>
          RemoveAssociationsOfModel<TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(
            ValueObjects.TimestampedId timestampedId,
            Func<ValueObjects.Id, Events.IAssociationRemovedEvent> newAssociationRemovedEvent,
            Func<IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getAssociationsOfModels,
            CancellationToken cancellationToken
            )
            where TAssociationModel : Models.IAssociation
            where TAggregate : class, IEventSourcedAggregate, new()
            where TAssociationAggregate : class, Aggregates.IAssociationAggregate, IConvertible<TAssociationModel>, new()
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
                         association.Timestamp, // TODO Casting to `TimestampedId` could result in a run-time error and must not be done!
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