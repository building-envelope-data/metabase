using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
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
                  componentId: command.Input.ComponentId,
                  information: ComponentInformationEventData.From(command.Input),
                  creatorId: command.CreatorId
                );
        }

        public Guid ComponentVersionId { get; set; }
        public Guid ComponentId { get; set; }
        public ComponentInformationEventData Information { get; set; }

#nullable disable
        public ComponentVersionCreated() { }
#nullable enable

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
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentVersionId, nameof(ComponentVersionId)),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  Information.Validate()
                  );
        }
    }
}