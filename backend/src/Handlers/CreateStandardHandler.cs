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
    public sealed class CreateStandardHandler
      : CreateModelHandler<Commands.CreateStandard, Aggregates.StandardAggregate>
    {
        public CreateStandardHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.CreateStandard command)
        {
            return Events.StandardCreated.From(id, command);
        }
    }
}