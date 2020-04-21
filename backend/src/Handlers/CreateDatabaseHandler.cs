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
    public sealed class CreateDatabaseHandler
      : CreateModelHandler<Commands.CreateDatabase, Aggregates.DatabaseAggregate>
    {
        public CreateDatabaseHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.CreateDatabase command)
        {
            return Events.DatabaseCreated.From(id, command);
        }
    }
}