using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class AddComponentManufacturerHandler
      : CreateModelHandler<Commands.AddComponentManufacturer, Aggregates.ComponentManufacturerAggregate>
    {
        public AddComponentManufacturerHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.AddComponentManufacturer command)
        {
            return Events.ComponentManufacturerAdded.From(id, command);
        }
    }
}