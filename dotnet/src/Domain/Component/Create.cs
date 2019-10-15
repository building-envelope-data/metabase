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
    public class Event : EventBase
    {
        public Guid ComponentId { get; set; }
    }

    public class Command : CommandBase<ComponentAggregate>
    {
    }

    public class CommandHandler : ICommandHandler<Command, ComponentAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentAggregate> Handle(Command command, CancellationToken cancellationToken)
        {
            var component = ComponentAggregate.Create(command.CreatorId);
            return await _repository.Store(component, cancellationToken);
        }
    }
}