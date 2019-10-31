using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;

namespace Icon.Domain.Component.Create
{
    public sealed class ComponentCreateEvent : EventBase
    {
        public Guid ComponentId { get; set; }

        public ComponentCreateEvent() { }

        public ComponentCreateEvent(Guid componentId, Command command) : base(command.CreatorId)
        {
            ComponentId = componentId;
        }
    }

    public sealed class Command : CommandBase<ComponentAggregate>
    {
        public Command(Guid creatorId) : base(creatorId) { }
    }

    public sealed class CommandHandler : ICommandHandler<Command, ComponentAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentAggregate> Handle(Command command, CancellationToken cancellationToken)
        {
            var @event = new ComponentCreateEvent(Guid.NewGuid(), command);
            var component = ComponentAggregate.Create(@event);
            return await _repository.Store(component, cancellationToken);
        }
    }
}