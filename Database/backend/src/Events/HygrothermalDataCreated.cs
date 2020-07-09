using Guid = System.Guid;

namespace Database.Events
{
    public sealed class HygrothermalDataCreated
      : DataCreatedEvent
    {
        public static HygrothermalDataCreated From(
            Guid hygrothermalDataId,
            Infrastructure.Commands.Create<ValueObjects.CreateHygrothermalDataInput> command
            )
        {
            return new HygrothermalDataCreated(
                hygrothermalDataId: hygrothermalDataId,
                componentId: command.Input.ComponentId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public HygrothermalDataCreated() { }
#nullable enable

        public HygrothermalDataCreated(
            Guid hygrothermalDataId,
            Guid componentId,
            object? data,
            Guid creatorId
            )
          : base(
              dataId: hygrothermalDataId,
              componentId: componentId,
              data: data,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}