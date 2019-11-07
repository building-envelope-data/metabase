using Guid = System.Guid;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using Events = Icon.Events;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using System.Linq;

namespace Icon.Handlers
{
    public sealed class CreateComponentHandler
      : ICommandHandler<Commands.CreateComponent, (Guid Id, DateTime Timestamp)>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public Task<(Guid Id, DateTime Timestamp)> Handle(Commands.CreateComponent command, CancellationToken cancellationToken)
        {
            // TODO Handle conflicting IDs
            var id = Guid.NewGuid();
            var @event = new Events.ComponentCreated(id, command);
            return _repository.Store<Aggregates.ComponentAggregate>(id, 1, @event, cancellationToken);
        }
    }
}