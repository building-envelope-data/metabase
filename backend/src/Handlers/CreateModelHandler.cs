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

namespace Icon.Handlers
{
    public class CreateModelHandler<TCommand, TAggregate>
      : ICommandHandler<TCommand, Result<ValueObjects.TimestampedId, Errors>>
      where TCommand : ICommand<Result<ValueObjects.TimestampedId, Errors>>
      where TAggregate : class, IEventSourcedAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Guid, TCommand, Events.IEvent> _newCreatedEvent;

        public CreateModelHandler(
            IAggregateRepository repository,
            Func<Guid, TCommand, Events.IEvent> newCreatedEvent
            )
        {
            _repository = repository;
            _newCreatedEvent = newCreatedEvent;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            TCommand command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(command, session, cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            TCommand command,
            IAggregateRepositorySession session,
            CancellationToken cancellationToken
            )
        {
            var id = await session.GenerateNewId(cancellationToken).ConfigureAwait(false);
            var @event = _newCreatedEvent(id, command);
            return
              await session.New<TAggregate>(
                  id, @event, cancellationToken
                  )
              .ConfigureAwait(false);
        }
    }
}