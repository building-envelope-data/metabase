using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class OpticalDataFromDatabase
      : NodeBase
    {
        public static OpticalDataFromDatabase FromModel(
            Models.OpticalDataFromDatabase model,
            Timestamp requestTimestamp
            )
        {
            return new OpticalDataFromDatabase(
                id: model.Id,
                databaseId: model.DatabaseId,
                componentId: model.ComponentId,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public object Data { get; }

        public OpticalDataFromDatabase(
            Id id,
            Id databaseId,
            Id componentId,
            object data,
            Timestamp timestamp,
            Timestamp requestTimestamp
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
            [Parent] OpticalDataFromDatabase opticalData,
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(opticalData.DatabaseId, opticalData.RequestTimestamp)
                );
        }

        public Task<Component> GetComponent(
            [Parent] OpticalDataFromDatabase opticalData,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(opticalData.ComponentId, opticalData.RequestTimestamp)
                );
        }
    }
}