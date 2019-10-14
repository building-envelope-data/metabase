using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;

namespace Icon.Domain.Component.Create
{
    public class Event : IEvent
    {
        public Guid ComponentId { get; set; }

        public Guid CreatorId { get; set; }
    }

    public class Command : ICommand<ComponentAggregate>
    {
        public Guid CreatorId { get; set; }
    }

    public class CommandHandler : ICommandHandler<Command, ComponentAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentAggregate> Handle(Command command, CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO Do we want to throw an exception here? (was ClientView in original code base)
            /* if (!_session.Query<UserView>().Any(c => c.Id == command.CreatorId)) */
            /*   throw new ArgumentException("Creator does not exist!", nameof(command.CreatorId)); */

          var component = ComponentAggregate.Create(command.CreatorId);
          return await _repository.Store(component, cancellationToken);
        }
    }
}