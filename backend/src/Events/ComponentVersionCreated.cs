using Guid = System.Guid;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;
using System.Collections.Generic;

namespace Icon.Events
{
    public sealed class ComponentVersionCreated
      : Event
    {
        public static ComponentVersionCreated From(
            Guid componentVersionId,
            Commands.CreateComponentVersion command
            )
        {
            return new ComponentVersionCreated(
                  componentVersionId: componentVersionId,
                  componentId: command.ComponentId,
                  information: ComponentInformationEventData.From(command.Information),
                  creatorId: command.CreatorId
                );
        }

        public Guid ComponentVersionId { get; set; }
        public Guid ComponentId { get; set; }
        public ComponentInformationEventData Information { get; }

        public ComponentVersionCreated() { }

        public ComponentVersionCreated(
            Guid componentVersionId,
            Guid componentId,
            ComponentInformationEventData information,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentVersionId = componentVersionId;
            ComponentId = componentId;
            Information = information;
        }

        public override bool IsValid()
        {
            return ComponentVersionId != Guid.Empty &&
              ComponentId != Guid.Empty &&
              Information.IsValid();
        }
    }
}