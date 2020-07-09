using Infrastructure.ValueObjects;
namespace Database.GraphQl
{
    public sealed class HygrothermalData
      : DataX
    {
        public static HygrothermalData FromModel(
            Models.HygrothermalData model,
            Timestamp requestTimestamp
            )
        {
            return new HygrothermalData(
                id: model.Id,
                componentId: model.ComponentId,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public HygrothermalData(
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