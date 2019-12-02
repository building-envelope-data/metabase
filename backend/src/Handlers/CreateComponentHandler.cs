using Guid = System.Guid;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
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
      : ICommandHandler<Commands.CreateComponent, Result<ValueObjects.TimestampedId, Errors>>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.CreateComponent command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                var id = await session.GenerateNewId(cancellationToken);
                var @event = Events.ComponentCreated.From(id, command);
                return
                  await session.Store<Aggregates.ComponentAggregate>(
                      id, 1, @event, cancellationToken
                      );
            }
        }
    }
}