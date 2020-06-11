using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Events;
using Icon.Infrastructure;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Commands = Icon.Commands;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentCreated
      : CreatedEvent
    {
        public static ComponentCreated From(
            Guid componentId,
            Commands.Create<ValueObjects.CreateComponentInput> command
            )
        {
            return new ComponentCreated(
                componentId: componentId,
                information: ComponentInformationEventData.From(command.Input.Information),
                creatorId: command.CreatorId
                );
        }

        public ComponentInformationEventData Information { get; set; }

#nullable disable
        public ComponentCreated() { }
#nullable enable

        public ComponentCreated(
            Guid componentId,
            ComponentInformationEventData information,
            Guid creatorId
            )
          : base(
              aggregateId: componentId,
              creatorId: creatorId
              )
        {
            Information = information;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  Information.Validate()
                  );
        }
    }
}