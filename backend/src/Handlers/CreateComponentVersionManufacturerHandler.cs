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
      : ICommandHandler<Commands.CreateComponentVersionManufacturer, Models.ComponentVersionManufacturer>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionManufacturerHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.ComponentVersionManufacturer> Handle(Commands.CreateComponentVersionManufacturer command, CancellationToken cancellationToken)
        {
            var @event = new Events.ComponentVersionManufacturerCreated(Guid.NewGuid(), command);
            var componentVersionManufacturer = Aggregates.ComponentVersionManufacturerAggregate.Create(@event);
            return (await _repository.Store(componentVersionManufacturer, cancellationToken))
              .ToModel();
        }
    }
}
