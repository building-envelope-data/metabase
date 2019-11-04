using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using Events = Icon.Events;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using System.Linq;

namespace Icon.Handlers
{
  public sealed class CreateComponentHandler
    : ICommandHandler<Commands.CreateComponent, Models.Component>
  {
    private readonly IAggregateRepository _repository;

    public CreateComponentHandler(IAggregateRepository repository)
    {
      _repository = repository;
    }

    public async Task<Models.Component> Handle(Commands.CreateComponent command, CancellationToken cancellationToken)
    {
      var @event = new Events.ComponentCreated(Guid.NewGuid(), command);
      var component = Aggregates.ComponentAggregate.Create(@event);
      return (await _repository.Store(component, cancellationToken)).ToModel();
    }
  }
}
