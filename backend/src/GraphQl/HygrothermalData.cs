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
    public sealed class HygrothermalData
      : DataX
    {
        public static HygrothermalData FromModel(
            Models.HygrothermalData model,
            ValueObjects.Timestamp requestTimestamp
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
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            object data,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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