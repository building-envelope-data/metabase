using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;

namespace Icon.Domain.ComponentVersionOwnership.Create
{
    public class Event : EventBase
    {
        public Guid ComponentVersionOwnershipId { get; set; }

        public Guid ComponentVersionId { get; set; }
    }

    public class Command : CommandBase<ComponentVersionOwnershipAggregate>
    {
        public Guid ComponentVersionId { get; set; }
    }

    public class CommandHandler : ICommandHandler<Command, ComponentVersionOwnershipAggregate>
    {
        private readonly IAggregateRepository _repository;

        public CommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ComponentVersionOwnershipAggregate> Handle(Command command, CancellationToken cancellationToken)
        {
            var componentVersionOwnership = ComponentVersionOwnershipAggregate.Create(command.ComponentVersionId, command.CreatorId);
            return await _repository.Store(componentVersionOwnership, cancellationToken);
        }
    }
}