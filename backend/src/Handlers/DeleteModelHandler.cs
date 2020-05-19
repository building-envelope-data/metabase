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
    public sealed class DeleteModelHandler<TModel, TAggregate>
      : ICommandHandler<Commands.Delete<TModel>, Result<ValueObjects.TimestampedId, Errors>>
      where TAggregate : class, IEventSourcedAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Commands.Delete<TModel>, Events.IDeletedEvent> _newDeletedEvent;

        public DeleteModelHandler(
            IAggregateRepository repository,
            Func<Commands.Delete<TModel>, Events.IDeletedEvent> newDeletedEvent
            )
        {
            _repository = repository;
            _newDeletedEvent = newDeletedEvent;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.Delete<TModel> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(command, session, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.Delete<TModel> command,
            IAggregateRepositorySession session,
            CancellationToken cancellationToken
            )
        {
            var @event = _newDeletedEvent(command);
            return
              await session.Delete<TAggregate>(
                  command.TimestampedId, @event, cancellationToken
                  )
              .ConfigureAwait(false);
        }
    }
}