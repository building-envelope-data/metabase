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
      : ICommandHandler<Commands.AddAssociation<TInput>, Result<ValueObjects.TimestampedId, Errors>>
      where TInput : ValueObjects.AddManyToManyAssociationInput
      where TAggregate : class, IEventSourcedAggregate, new()
      where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Guid, Commands.AddAssociation<TInput>, Events.IAssociationAddedEvent> _newAssociationAddedEvent;

        public AddManyToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Commands.AddAssociation<TInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent
            )
        {
            _repository = repository;
            _newAssociationAddedEvent = newAssociationAddedEvent;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.AddAssociation<TInput> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(session, command, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            IAggregateRepositorySession session,
            Commands.AddAssociation<TInput> command,
            CancellationToken cancellationToken
            )
        {
            return await (
              await session.AddManyToManyAssociation<TAggregate, TAssociationAggregate, TAssociateAggregate>(
                id => _newAssociationAddedEvent(id, command),
                AddAssociationCheck.PARENT_AND_ASSOCIATE,
                cancellationToken
                )
                .ConfigureAwait(false)
              )
                .Bind(async id => await
                    (await session.Save(cancellationToken).ConfigureAwait(false))
                    .Bind(async _ =>
                      await session.TimestampId<TAggregate>(id, cancellationToken).ConfigureAwait(false)
                      )
                    .ConfigureAwait(false)
                    )
                .ConfigureAwait(false);
        }
    }
}