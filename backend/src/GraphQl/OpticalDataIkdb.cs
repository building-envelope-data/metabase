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
    public sealed class OpticalDataIkdb
      : NodeBase
    {
        public static OpticalDataIkdb FromModel(
            Models.OpticalDataIkdb model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new OpticalDataIkdb(
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

        public OpticalDataIkdb(
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
            [Parent] OpticalDataIkdb opticalData,
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(opticalData.DatabaseId, opticalData.RequestTimestamp)
                );
        }

        public Task<Component> GetComponent(
            [Parent] OpticalDataIkdb opticalData,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(opticalData.ComponentId, opticalData.RequestTimestamp)
                );
        }
    }
}