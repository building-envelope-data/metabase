using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class HygrothermalData
      : Node
    {
        public static HygrothermalData FromModel(
            Models.HygrothermalData model,
            Timestamp requestTimestamp
            )
        {
            return new HygrothermalData(
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

        public HygrothermalData(
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
            [Parent] HygrothermalData hygrothermalData,
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(hygrothermalData.DatabaseId, hygrothermalData.RequestTimestamp)
                );
        }

        public Task<Component> GetComponent(
            [Parent] HygrothermalData hygrothermalData,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(hygrothermalData.ComponentId, hygrothermalData.RequestTimestamp)
                );
        }
    }
}