using Guid = System.Guid;

namespace Database.Events
{
    public sealed class CalorimetricDataCreated
      : DataCreatedEvent
    {
        public static CalorimetricDataCreated From(
            Guid opticalDataId,
            Infrastructure.Commands.CreateCommand<ValueObjects.CreateCalorimetricDataInput> command
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