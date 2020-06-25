using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class CalorimetricDataCreated
      : DataCreatedEvent
    {
        public static CalorimetricDataCreated From(
            Guid opticalDataId,
            Commands.Create<ValueObjects.CreateCalorimetricDataInput> command
            )
        {
            return new CalorimetricDataCreated(
                opticalDataId: opticalDataId,
                componentId: command.Input.ComponentId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public CalorimetricDataCreated() { }
#nullable enable

        public CalorimetricDataCreated(
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