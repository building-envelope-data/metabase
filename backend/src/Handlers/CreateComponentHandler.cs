using Guid = System.Guid;
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
      : ICommandHandler<Commands.CreateComponent, Guid>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(Commands.CreateComponent command, CancellationToken cancellationToken)
        {
            // TODO Handle conflicting IDs
            var id = Guid.NewGuid();
            var @event = new Events.ComponentCreated(id, command);
            return _repository.Store(id, 0, @event, cancellationToken);
        }
    }
}