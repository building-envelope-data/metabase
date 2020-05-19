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
      : ICommandHandler<Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Result<ValueObjects.TimestampedId, Errors>>
      where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> _newRemovedEvent;

        public RemoveManyToManyAssociationHandler(
            IAggregateRepository repository,
            Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> newRemovedEvent
            )
        {
            _repository = repository;
            _newRemovedEvent = newRemovedEvent;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(command, session, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>> command,
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
                      var @event = _newRemovedEvent(associationId, command);
                      return await session.Delete<TAssociationAggregate>(
                          associationId, command.Input.Timestamp, @event, cancellationToken
                          )
                      .ConfigureAwait(false);
                  }
                  ).ConfigureAwait(false);
        }
    }
}