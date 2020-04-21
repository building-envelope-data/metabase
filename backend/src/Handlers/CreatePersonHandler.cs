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
    public sealed class CreatePersonHandler
      : CreateModelHandler<Commands.CreatePerson, Aggregates.PersonAggregate>
    {
        public CreatePersonHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.CreatePerson command)
        {
            return Events.PersonCreated.From(id, command);
        }
    }
}