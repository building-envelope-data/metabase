using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class OpticalData
      : DataX
    {
        public static OpticalData FromModel(
            Models.OpticalData model,
            Timestamp requestTimestamp
            )
        {
            return new OpticalData(
                id: model.Id,
                componentId: model.ComponentId,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public OpticalData(
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