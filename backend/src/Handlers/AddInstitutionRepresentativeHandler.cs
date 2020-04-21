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
    public sealed class AddInstitutionRepresentativeHandler
      : CreateModelHandler<Commands.AddInstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>
    {
        public AddInstitutionRepresentativeHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.AddInstitutionRepresentative command)
        {
            return Events.InstitutionRepresentativeAdded.From(id, command);
        }
    }
}