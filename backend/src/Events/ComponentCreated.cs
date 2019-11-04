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
    public sealed class ComponentCreated : EventBase
    {
        public Guid ComponentId { get; set; }

        public ComponentCreated() { }

        public ComponentCreated(Guid componentId, Commands.CreateComponent command) : base(command.CreatorId)
        {
            ComponentId = componentId;
        }
    }
}