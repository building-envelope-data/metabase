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
                data: model.Data,
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

        public Task<IReadOnlyList<Component>> GetComponents(
            [Parent] OpticalData opticalData,
            [DataLoader] ComponentsOfOpticalDataDataLoader componentsLoader
            )
        {
            return componentsLoader.LoadAsync(
                TimestampHelpers.TimestampId(opticalData.Id, opticalData.RequestTimestamp)
                );
        }

        public sealed class ComponentsOfOpticalDataDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Component, Models.OpticalData, Models.ComponentOpticalData, Models.Component>
        {
            public ComponentsOfOpticalDataDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}