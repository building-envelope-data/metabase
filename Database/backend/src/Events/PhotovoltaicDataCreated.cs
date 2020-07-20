using Guid = System.Guid;

namespace Database.Events
{
    public sealed class PhotovoltaicDataCreated
      : DataCreatedEvent
    {
        public static PhotovoltaicDataCreated From(
            Guid photovoltaicDataId,
            Infrastructure.Commands.CreateCommand<ValueObjects.CreatePhotovoltaicDataInput> command
            )
        {
            return new PhotovoltaicDataCreated(
                photovoltaicDataId: photovoltaicDataId,
                componentId: command.Input.ComponentId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PhotovoltaicDataCreated() { }
#nullable enable

        public PhotovoltaicDataCreated(
            Guid photovoltaicDataId,
            Guid componentId,
            object? data,
            Guid creatorId
            )
          : base(
              dataId: photovoltaicDataId,
              componentId: componentId,
              data: data,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}