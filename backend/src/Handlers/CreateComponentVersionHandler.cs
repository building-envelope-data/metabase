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
      : ICommandHandler<Commands.CreateComponentVersion, Guid>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(Commands.CreateComponentVersion command, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var @event = new Events.ComponentVersionCreated(id, command);
            return _repository.Store(id, 0, @event, cancellationToken);
        }
    }
}