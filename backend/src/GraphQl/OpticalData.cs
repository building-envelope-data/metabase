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
    public sealed class OpticalData
      : NodeBase
    {
        public static OpticalData FromModel(
            Models.OpticalData model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new OpticalData(
                id: model.Id,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public object Data { get; }

        public OpticalData(
            ValueObjects.Id id,
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
            Data = data;
        }

        public async Task<ValueObjects.Id> GetComponentId(
            [Parent] OpticalData opticalData,
            [DataLoader] ComponentOfOpticalDataDataLoader componentLoader
            )
        {
            return
              (
               await componentLoader.LoadAsync(
                 TimestampHelpers.TimestampId(opticalData.Id, opticalData.RequestTimestamp)
                 )
              )
              .Id;
        }

        public sealed class ComponentOfOpticalDataDataLoader
            : BackwardOneToManyAssociateOfModelDataLoader<Component, Models.OpticalData, Models.ComponentOpticalData, Models.Component>
        {
            public ComponentOfOpticalDataDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}