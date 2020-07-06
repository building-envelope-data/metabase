using System; // Func
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Metabase.Handlers
{
    public sealed class AddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : ICommandHandler<Commands.AddAssociation<TInput>, Result<TimestampedId, Errors>>
      where TInput : ValueObjects.AddManyToManyAssociationInput
      where TAggregate : class, IEventSourcedAggregate, new()
      where TAssociationAggregate : class, IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Guid, Commands.AddAssociation<TInput>, IAssociationAddedEvent> _newAssociationAddedEvent;

        public AddManyToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Commands.AddAssociation<TInput>, IAssociationAddedEvent> newAssociationAddedEvent
            )
        {
            _repository = repository;
            _newAssociationAddedEvent = newAssociationAddedEvent;
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            Commands.AddAssociation<TInput> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(session, command, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
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
                      await session.TimestampId<TAssociationAggregate>(id, cancellationToken).ConfigureAwait(false)
                      )
                    .ConfigureAwait(false)
                    )
                .ConfigureAwait(false);
        }
    }
}