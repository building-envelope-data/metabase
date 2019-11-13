using Guid = System.Guid;
using DateTime = System.DateTime;
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
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class CreateComponentHandler
      : ICommandHandler<Commands.CreateComponent, Result<(Guid Id, DateTime Timestamp), IError>>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<(Guid Id, DateTime Timestamp), IError>> Handle(
            Commands.CreateComponent command,
            CancellationToken cancellationToken
            )
        {
            // TODO Handle conflicting IDs
            var id = Guid.NewGuid();
            var @event = new Events.ComponentCreated(id, command);
            using (var session = _repository.OpenSession())
            {
                return await session.Store<Aggregates.ComponentAggregate>(id, 1, @event, cancellationToken);
            }
        }
    }
}