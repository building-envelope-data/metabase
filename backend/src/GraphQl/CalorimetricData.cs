namespace Icon.GraphQl
{
    public sealed class CalorimetricData
      : DataX
    {
        public static CalorimetricData FromModel(
            Models.CalorimetricData model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new CalorimetricData(
                id: model.Id,
                componentId: model.ComponentId,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public CalorimetricData(
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