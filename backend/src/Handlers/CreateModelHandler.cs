using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public abstract class CreateModelHandler<C, A>
      : ICommandHandler<C, Result<ValueObjects.TimestampedId, Errors>>
      where C : ICommand<Result<ValueObjects.TimestampedId, Errors>>
      where A : class, IEventSourcedAggregate, new()
    {
        private readonly IAggregateRepository _repository;

        public CreateModelHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            C command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(command, session, cancellationToken);
            }
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            C command,
            IAggregateRepositorySession session,
            CancellationToken cancellationToken
            )
        {
            var id = await session.GenerateNewId(cancellationToken);
            var @event = NewCreatedEvent(id, command);
            return
              await session.New<A>(
                  id, @event, cancellationToken
                  );
        }

        protected abstract Events.IEvent NewCreatedEvent(ValueObjects.Id id, C command);
    }
}