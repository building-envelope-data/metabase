using System; // Func
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public sealed class RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>
      : ICommandHandler<Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Result<ValueObjects.TimestampedId, Errors>>
      where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> _newAssociationRemovedEvent;

        public RemoveManyToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
            )
        {
            _repository = repository;
            _newAssociationRemovedEvent = newAssociationRemovedEvent;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>> command,
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
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>> command,
            CancellationToken cancellationToken
            )
        {
            return await
              (
               await session.RemoveManyToManyAssociation<TAssociationAggregate>(
                 (command.Input.ParentId, command.Input.AssociateId),
                 command.Input.Timestamp,
                 id => _newAssociationRemovedEvent(id, command),
                 cancellationToken
                 )
               .ConfigureAwait(false)
              )
              .Bind(async associationId => await
                  (await session.Save(cancellationToken).ConfigureAwait(false))
                  .Bind(async _ => await
                      session.TimestampId<TAssociationAggregate>(associationId, cancellationToken).ConfigureAwait(false)
                      )
                  .ConfigureAwait(false)
                  )
              .ConfigureAwait(false);
        }
    }
}