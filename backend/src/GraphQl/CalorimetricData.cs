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
    public sealed class CalorimetricData
      : NodeBase
    {
        public static CalorimetricData FromModel(
            Models.CalorimetricData model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new CalorimetricData(
                id: model.Id,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public object Data { get; }

        public CalorimetricData(
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
            [Parent] CalorimetricData calorimetricData,
            [DataLoader] ComponentOfCalorimetricDataDataLoader componentLoader
            )
        {
            return
              (
               await componentLoader.LoadAsync(
                 TimestampHelpers.TimestampId(calorimetricData.Id, calorimetricData.RequestTimestamp)
                 )
              )
              .Id;
        }

        public sealed class ComponentOfCalorimetricDataDataLoader
            : BackwardOneToManyAssociateOfModelDataLoader<Component, Models.CalorimetricData, Models.ComponentCalorimetricData, Models.Component>
        {
            public ComponentOfCalorimetricDataDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}