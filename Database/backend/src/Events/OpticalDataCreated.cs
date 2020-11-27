using Guid = System.Guid;

namespace Database.Events
{
    public sealed class OpticalDataCreated
      : DataCreatedEvent
    {
        public static OpticalDataCreated From(
            Guid opticalDataId,
            Infrastructure.Commands.CreateCommand<ValueObjects.CreateOpticalDataInput> command
            )
        {
            return new OpticalDataCreated(
                opticalDataId: opticalDataId,
                componentId: command.Input.ComponentId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public OpticalDataCreated() { }
#nullable enable

        public OpticalDataCreated(
            Guid opticalDataId,
            Guid componentId,
            object? data,
            Guid creatorId
            )
          : base(
              dataId: opticalDataId,
              componentId: componentId,
              data: data,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}