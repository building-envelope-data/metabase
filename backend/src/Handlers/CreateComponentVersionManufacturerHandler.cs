using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using DateTime = System.DateTime;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using Events = Icon.Events;

namespace Icon.Handlers
{
    public sealed class CreateComponentVersionManufacturerHandler
      : ICommandHandler<Commands.CreateComponentVersionManufacturer, Guid>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionManufacturerHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(Commands.CreateComponentVersionManufacturer command, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var @event = new Events.ComponentVersionManufacturerCreated(id, command);
            return _repository.Store(id, 0, @event, cancellationToken);
        }
    }
}