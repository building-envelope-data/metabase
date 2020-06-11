using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class PhotovoltaicDataFromDatabase
      : NodeBase
    {
        public static PhotovoltaicDataFromDatabase FromModel(
            Models.PhotovoltaicDataFromDatabase model,
            ValueObjects.Timestamp requestTimestamp
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

        public ValueObjects.Id DatabaseId { get; }
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public PhotovoltaicDataFromDatabase(
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