using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class PhotovoltaicData
      : DataX
    {
        public static PhotovoltaicData FromModel(
            Models.PhotovoltaicData model,
            Timestamp requestTimestamp
            )
        {
            return new PhotovoltaicData(
                id: model.Id,
                componentId: model.ComponentId,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public PhotovoltaicData(
            Id id,
            Id componentId,
            object data,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              componentId: componentId,
              data: data,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}