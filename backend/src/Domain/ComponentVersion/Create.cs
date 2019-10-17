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
    public class Event : EventBase
    {
        public Guid ComponentVersionId { get; set; }
        public Guid ComponentId { get; set; }

        public static Event From(Guid componentVersionId, Command command)
        {
           return new Event
           {
                ComponentVersionId = componentVersionId,
                ComponentId = command.ComponentId,
                CreatorId = command.CreatorId,
           };
        }
    }

    public class Command : CommandBase<ComponentVersionAggregate>
    {
        public Guid ComponentId { get; set; }
    }

    public class CommandHandler : ICommandHandler<Command, ComponentVersionAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentVersionAggregate> Handle(Command command, CancellationToken cancellationToken)
        {
            var @event = Event.From(Guid.NewGuid(), command);
            var componentVersion = ComponentVersionAggregate.Create(@event);
            return await _repository.Store(componentVersion, cancellationToken);
        }
    }
}