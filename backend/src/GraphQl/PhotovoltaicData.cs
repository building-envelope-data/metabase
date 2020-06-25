namespace Icon.GraphQl
{
    public sealed class PhotovoltaicData
      : DataX
    {
        public static PhotovoltaicData FromModel(
            Models.PhotovoltaicData model,
            ValueObjects.Timestamp requestTimestamp
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
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            object data,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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