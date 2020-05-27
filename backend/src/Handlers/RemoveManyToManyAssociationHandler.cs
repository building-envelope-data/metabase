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
    public sealed class RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>
      : ICommandHandler<Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Result<ValueObjects.TimestampedId, Errors>>
      where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, new()
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
                return await Handle(command, session, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>> command,
            IAggregateRepositorySession session,
            CancellationToken cancellationToken
            )
        {
            var maybeAssociationId =
              await session.Query<TAssociationAggregate>()
              .Where(a =>
                  a.ParentId == command.Input.ParentId &&
                  a.AssociateId == command.Input.AssociateId
                  )
              .Select(a => a.Id)
              .FirstOrDefaultAsync(cancellationToken);
            return
              await ValueObjects.Id.From(maybeAssociationId)
              .Bind(async associationId =>
                  {
                      var @event = _newAssociationRemovedEvent(associationId, command);
                      return await (
                          await session.Delete<TAssociationAggregate>(
                            associationId, command.Input.Timestamp, @event, cancellationToken
                            )
                          .ConfigureAwait(false)
                          )
                      .Bind(async _ => await
                          (await session.Save(cancellationToken).ConfigureAwait(false))
                          .Bind(async _ => await
                            session.TimestampId<TAssociationAggregate>(associationId, cancellationToken).ConfigureAwait(false)
                            )
                          .ConfigureAwait(false)
                          )
                      .ConfigureAwait(false);
                  }
                  )
              .ConfigureAwait(false);
        }
    }
}