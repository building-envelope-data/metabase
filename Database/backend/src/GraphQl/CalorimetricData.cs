using Infrastructure.ValueObjects;
namespace Database.GraphQl
{
    public sealed class CalorimetricData
      : DataX
    {
        public static CalorimetricData FromModel(
            Models.CalorimetricData model,
            Timestamp requestTimestamp
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