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

        public Task<Result<(Guid Id, DateTime Timestamp), IError>> Handle(
            Commands.CreateComponentVersion command,
            CancellationToken cancellationToken
            )
        {
            var id = Guid.NewGuid();
            var @event = new Events.ComponentVersionCreated(id, command);
            return _repository.Store<Aggregates.ComponentVersionAggregate>(id, 1, @event, cancellationToken);
        }
    }
}