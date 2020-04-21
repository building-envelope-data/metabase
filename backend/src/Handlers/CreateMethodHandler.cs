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
    public sealed class CreateMethodHandler
      : CreateModelHandler<Commands.CreateMethod, Aggregates.MethodAggregate>
    {
        public CreateMethodHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.CreateMethod command)
        {
            return Events.MethodCreated.From(id, command);
        }
    }
}