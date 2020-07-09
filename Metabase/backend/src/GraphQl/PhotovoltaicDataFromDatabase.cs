using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class PhotovoltaicDataFromDatabase
      : NodeBase
    {
        public static PhotovoltaicDataFromDatabase FromModel(
            Models.PhotovoltaicDataFromDatabase model,
            Timestamp requestTimestamp
            )
        {
            return new PhotovoltaicDataFromDatabase(
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

        public PhotovoltaicDataFromDatabase(
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
            [Parent] PhotovoltaicDataFromDatabase photovoltaicData,
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(photovoltaicData.DatabaseId, photovoltaicData.RequestTimestamp)
                );
        }

        public Task<Component> GetComponent(
            [Parent] PhotovoltaicDataFromDatabase photovoltaicData,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(photovoltaicData.ComponentId, photovoltaicData.RequestTimestamp)
                );
        }
    }
}