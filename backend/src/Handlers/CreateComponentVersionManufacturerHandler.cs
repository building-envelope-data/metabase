using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using DateTime = System.DateTime;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using Events = Icon.Events;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class CreateComponentVersionManufacturerHandler
      : ICommandHandler<Commands.CreateComponentVersionManufacturer, Result<(Guid Id, DateTime Timestamp), IError>>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionManufacturerHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<(Guid Id, DateTime Timestamp), IError>> Handle(
            Commands.CreateComponentVersionManufacturer command,
            CancellationToken cancellationToken
            )
        {
            var id = Guid.NewGuid();
            var @event = new Events.ComponentVersionManufacturerCreated(id, command);
            using (var session = _repository.OpenSession())
            {
                return session.Store<Aggregates.ComponentVersionManufacturerAggregate>(id, 1, @event, cancellationToken);
            }
        }
    }
}