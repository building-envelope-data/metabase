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
using Aggregates = Icon.Aggregates;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class CreateComponentVersionHandler
      : ICommandHandler<Commands.CreateComponentVersion, Result<(Guid Id, DateTime Timestamp), IError>>
    {
        private readonly IAggregateRepository _repository;

        public CreateComponentVersionHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<(Guid Id, DateTime Timestamp), IError>> Handle(
            Commands.CreateComponentVersion command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                var id = await session.GenerateNewId(cancellationToken);
                var @event = Events.ComponentVersionCreated.From(id, command);
                return
                  await session.Store<Aggregates.ComponentVersionAggregate>(
                      id, 1, @event, cancellationToken
                      );
            }
        }
    }
}