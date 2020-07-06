using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class PhotovoltaicDataCreated
      : DataCreatedEvent
    {
        public static PhotovoltaicDataCreated From(
            Guid photovoltaicDataId,
            Commands.Create<ValueObjects.CreatePhotovoltaicDataInput> command
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