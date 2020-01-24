using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
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
      : ICommandHandler<Commands.CreateComponentVersionManufacturer, Result<ValueObjects.TimestampedId, Errors>>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionManufacturerHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.CreateComponentVersionManufacturer command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                var id = await session.GenerateNewId(cancellationToken);
                var @event = Events.ComponentVersionManufacturerCreated.From(id, command);
                return
                  await session.Store<Aggregates.ComponentVersionManufacturerAggregate>(
                      id, 1, @event, cancellationToken
                      );
            }
        }
    }
}