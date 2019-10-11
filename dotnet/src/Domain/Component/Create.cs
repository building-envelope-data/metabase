using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Domain;

namespace Icon.Domain.Component.Create
{
    public class Event: IEvent
    {
        public Guid ComponentId { get; set; }

        public Guid CreatorId { get; set; }
    }

    public class Command: ICommand<ComponentAggregate>
    {
        public Guid CreatorId { get; set; }
    }

    public class CommandHandler: ICommandHandler<Command, ComponentAggregate>{
        private readonly Marten.IDocumentSession _session;
        private readonly IEventBus _eventBus;

        private Marten.Events.IEventStore store => _session.Events;

        public CommandHandler(
            Marten.IDocumentSession session,
            IEventBus eventBus)
        {
            _session = session;
            _eventBus = eventBus;
        }

    public async Task<ComponentAggregate> Handle(Command command, CancellationToken cancellationToken = default(CancellationToken))
    {
      // TODO Do we want to throw an exception here? (was ClientView in original code base)
      /* if (!_session.Query<UserView>().Any(c => c.Id == command.CreatorId)) */
      /*   throw new ArgumentException("Creator does not exist!", nameof(command.CreatorId)); */

      var component = ComponentAggregate.Create(command.CreatorId);

      store.Append(component.Id, component.PendingEvents.ToArray());
      await _session.SaveChangesAsync(cancellationToken);
      await _eventBus.Publish(component.PendingEvents.ToArray());

      return component;
    }
    }
}
