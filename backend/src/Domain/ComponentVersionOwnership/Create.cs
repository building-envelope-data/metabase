using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;

namespace Icon.Domain.ComponentVersionOwnership.Create
{
    public sealed class Data
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        /* public DateInterval Availability { get; set; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
        public DateTime AvailableFrom { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime AvailableUntil { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
    }

    public sealed class Event : EventBase
    {
        public Guid ComponentVersionOwnershipId { get; private set; }
        public Guid ComponentVersionId { get; private set; }
        public Data Data { get; private set; }

        public Event(Guid componentVersionOwnershipId, Command command) : base(command.CreatorId)
        {
                ComponentVersionOwnershipId = componentVersionOwnershipId;
                ComponentVersionId = command.ComponentVersionId;
                Data = command.Data;
        }
    }

    public sealed class Command : CommandBase<ComponentVersionOwnershipAggregate>
    {
        public Guid ComponentVersionId { get; private set; }
        public Data Data { get; private set; }

        public static Command From(Guid componentVersionId, Data data, Guid creatorId)
        {
          return new Command
          {
            ComponentVersionId = componentVersionId,
            Data = data,
            CreatorId = creatorId,
          };
        }
    }

    public sealed class CommandHandler : ICommandHandler<Command, ComponentVersionOwnershipAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentVersionOwnershipAggregate> Handle(Command command, CancellationToken cancellationToken)
        {
            var @event = new Event(Guid.NewGuid(), command);
            var componentVersionOwnership = ComponentVersionOwnershipAggregate.Create(@event);
            return await _repository.Store(componentVersionOwnership, cancellationToken);
        }
    }
}