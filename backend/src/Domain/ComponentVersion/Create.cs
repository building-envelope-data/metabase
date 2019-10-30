using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;

namespace Icon.Domain.ComponentVersion.Create
{
    public sealed class ComponentVersionCreateEvent : EventBase
    {
        public Guid ComponentVersionId { get; set; }
        public Guid ComponentId { get; set; }

        public ComponentVersionCreateEvent() { }

        public ComponentVersionCreateEvent(Guid componentVersionId, Command command) : base(command.CreatorId)
        {
            ComponentVersionId = componentVersionId;
            ComponentId = command.ComponentId;
        }
    }

    public sealed class Command : CommandBase<ComponentVersionAggregate>
    {
        public Command(Guid creatorId) : base(creatorId) { }

        public Guid ComponentId { get; set; }
    }

    public sealed class CommandHandler : ICommandHandler<Command, ComponentVersionAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentVersionAggregate> Handle(Command command, CancellationToken cancellationToken)
        {
            var @event = new ComponentVersionCreateEvent(Guid.NewGuid(), command);
            var componentVersion = ComponentVersionAggregate.Create(@event);
            return await _repository.Store(componentVersion, cancellationToken);
        }
    }
}