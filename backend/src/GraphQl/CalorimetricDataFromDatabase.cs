using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CalorimetricDataFromDatabase
      : NodeBase
    {
        public static CalorimetricDataFromDatabase FromModel(
            Models.CalorimetricDataFromDatabase model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new CalorimetricDataFromDatabase(
                id: model.Id,
                databaseId: model.DatabaseId,
                componentId: model.ComponentId,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ValueObjects.Id DatabaseId { get; }
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public CalorimetricDataFromDatabase(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            object data,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public Task<Database> GetDatabase(
            [Parent] CalorimetricDataFromDatabase calorimetricData,
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(calorimetricData.DatabaseId, calorimetricData.RequestTimestamp)
                );
        }

        public Task<Component> GetComponent(
            [Parent] CalorimetricDataFromDatabase calorimetricData,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(calorimetricData.ComponentId, calorimetricData.RequestTimestamp)
                );
        }
    }
}