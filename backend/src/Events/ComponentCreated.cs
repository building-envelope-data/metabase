using Icon;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using System.Collections.Generic;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class ComponentCreated : Event
    {
        public static ComponentCreated From(
            Guid componentId,
            Commands.CreateComponent command
            )
        {
            return new ComponentCreated(
                componentId: componentId,
                information: ComponentInformationEventData.From(command.Input.Information),
                creatorId: command.CreatorId
                );
        }

        public Guid ComponentId { get; set; }
        public ComponentInformationEventData Information { get; set; }

#nullable disable
        public ComponentCreated() { }
#nullable enable

        public ComponentCreated(
            Guid componentId,
            ComponentInformationEventData information,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentId = componentId;
            Information = information;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  Information.Validate()
                  );
        }
    }
}