using CSharpFunctionalExtensions;
using Infrastructure.Events;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Events
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