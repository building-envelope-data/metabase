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
    public sealed class HygrothermalDataFromDatabase
      : NodeBase
    {
        public static HygrothermalDataFromDatabase FromModel(
            Models.HygrothermalDataFromDatabase model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new HygrothermalDataFromDatabase(
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

        public HygrothermalDataFromDatabase(
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
            [Parent] HygrothermalDataFromDatabase hygrothermalData,
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(hygrothermalData.DatabaseId, hygrothermalData.RequestTimestamp)
                );
        }

        public Task<Component> GetComponent(
            [Parent] HygrothermalDataFromDatabase hygrothermalData,
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(hygrothermalData.ComponentId, hygrothermalData.RequestTimestamp)
                );
        }
    }
}