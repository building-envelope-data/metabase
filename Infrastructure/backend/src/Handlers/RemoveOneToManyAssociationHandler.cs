using System; // Func
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Handlers
{
    public sealed class RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>
      : ICommandHandler<Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, Result<TimestampedId, Errors>>
      where TAssociationAggregate : class, IOneToManyAssociationAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Guid, Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> _newAssociationRemovedEvent;

        public RemoveOneToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
            )
        {
            _repository = repository;
            _newAssociationRemovedEvent = newAssociationRemovedEvent;
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>> command,
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
            Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>> command,
            CancellationToken cancellationToken
            )
        {
            return await
              (
               await session.RemoveOneToManyAssociation<TAssociationAggregate>(
                 command.Input.AssociateId,
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