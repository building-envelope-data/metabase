using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;

namespace Icon.Handlers
{
    public sealed class CreateComponentVersionHandler
      : ICommandHandler<Commands.CreateComponentVersion, Models.ComponentVersion>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.ComponentVersion> Handle(Commands.CreateComponentVersion command, CancellationToken cancellationToken)
        {
            var @event = new Events.ComponentVersionCreated(Guid.NewGuid(), command);
            var componentVersion = Aggregates.ComponentVersionAggregate.Create(@event);
            return (await _repository.Store(componentVersion, cancellationToken)).ToModel();
        }
    }
}