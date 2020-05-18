using System.Threading.Tasks;
using System; // Func
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using CSharpFunctionalExtensions;
using Marten;
using Marten.Linq;
using System.Linq;

namespace Icon.Handlers
{
    public sealed class AddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : CreateModelHandler<Commands.Add<TInput>, TAssociationAggregate>
      where TInput : ValueObjects.AddManyToManyAssociationInput
      where TAggregate : class, IEventSourcedAggregate, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, new()
    {
        public AddManyToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Commands.Add<TInput>, Events.IEvent> newAddedEvent
            )
          : base(
              repository,
              newAddedEvent
              )
        {
        }

        public override async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.Add<TInput> command,
            IAggregateRepositorySession session,
            CancellationToken cancellationToken
            )
        {
            // TODO Do the queries below in a Marten batch!
            var parentResult = await session.Load<TAggregate>(
                                    command.Input.ParentId,
                                    cancellationToken
                                    );
            var associateResult = await session.Load<TAssociateAggregate>(
                                    command.Input.AssociateId,
                                    cancellationToken
                                    );
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
                    return await base.Handle(command, session, cancellationToken).ConfigureAwait(false);
                }
                )
                                    .ConfigureAwait(false);
        }
    }
}