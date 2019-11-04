using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class ComponentVersionCreated
      : EventBase
    {
        public Guid ComponentVersionId { get; set; }
        public Guid ComponentId { get; set; }

        public ComponentVersionCreated() { }

        public ComponentVersionCreated(Guid componentVersionId, Commands.CreateComponentVersion command) : base(command.CreatorId)
        {
            ComponentVersionId = componentVersionId;
            ComponentId = command.ComponentId;
        }
    }
}
