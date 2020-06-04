using System; // Func
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Marten;
using Marten.Linq;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using Commands = Icon.Commands;
using DateTime = System.DateTime;
using Events = Icon.Events;

namespace Icon.Handlers
{
    public sealed class AddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : CreateModelHandler<Commands.AddAssociation<TInput>, TAssociationAggregate>
      where TInput : ValueObjects.AddManyToManyAssociationInput
      where TAggregate : class, IEventSourcedAggregate, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, new()
    {
        public AddManyToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Commands.AddAssociation<TInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent
            )
          : base(
              repository,
              newAssociationAddedEvent,
              Enumerable.Empty<Func<IAggregateRepositorySession, Commands.AddAssociation<TInput>, CancellationToken, Task<Result<bool, Errors>>>>()
              )
        {
        }

        public override async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            IAggregateRepositorySession session,
            Commands.AddAssociation<TInput> command,
            CancellationToken cancellationToken
            )
        {
            var (parentResult, associateResult) =
              await session.Load<TAggregate, TAssociateAggregate>(
                  (
                   command.Input.ParentId,
                   command.Input.AssociateId
                  ),
                  cancellationToken
                  )
              .ConfigureAwait(false);
            return
              await Errors.Combine(
                  parentResult,
                  associateResult
                  )
              .Bind(async _ =>
                {
                    var doesAssociationExist =
                                    await session.Query<TAssociationAggregate>()
                    .Where(a =>
                        a.ParentId == command.Input.ParentId &&
                        a.AssociateId == command.Input.AssociateId
                        )
                    .AnyAsync(cancellationToken)
                    .ConfigureAwait(false);
                    if (doesAssociationExist)
                    {
                        return Result.Failure<ValueObjects.TimestampedId, Errors>(
                            Errors.One(
                              message: $"There already is an association from {command.Input.ParentId} to {command.Input.AssociateId} of type {typeof(TAssociationAggregate)}",
                              code: ErrorCodes.AlreadyExistingAssociation
                              )
                            );
                    }
                    return await base.Handle(session, command, cancellationToken).ConfigureAwait(false);
                }
                )
                                    .ConfigureAwait(false);
        }
    }
}