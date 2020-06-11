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
      : NodeBase
    {
        public static HygrothermalData FromModel(
            Models.HygrothermalData model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new HygrothermalData(
                id: model.Id,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public object Data { get; }

        public HygrothermalData(
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
            [Parent] HygrothermalData hygrothermalData,
            [DataLoader] ComponentOfHygrothermalDataDataLoader componentLoader
            )
        {
            return
              (
               await componentLoader.LoadAsync(
                 TimestampHelpers.TimestampId(hygrothermalData.Id, hygrothermalData.RequestTimestamp)
                 )
              )
              .Id;
        }

        public sealed class ComponentOfHygrothermalDataDataLoader
            : BackwardOneToManyAssociateOfModelDataLoader<Component, Models.HygrothermalData, Models.ComponentHygrothermalData, Models.Component>
        {
            public ComponentOfHygrothermalDataDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}