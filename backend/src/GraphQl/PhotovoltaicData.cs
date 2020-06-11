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
    public sealed class PhotovoltaicData
      : NodeBase
    {
        public static PhotovoltaicData FromModel(
            Models.PhotovoltaicData model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new PhotovoltaicData(
                id: model.Id,
                data: model.Data.ToNestedCollections(),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public object Data { get; }

        public PhotovoltaicData(
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
            [Parent] PhotovoltaicData photovoltaicData,
            [DataLoader] ComponentOfPhotovoltaicDataDataLoader componentLoader
            )
        {
            return
              (
               await componentLoader.LoadAsync(
                 TimestampHelpers.TimestampId(photovoltaicData.Id, photovoltaicData.RequestTimestamp)
                 )
              )
              .Id;
        }

        public sealed class ComponentOfPhotovoltaicDataDataLoader
            : BackwardOneToManyAssociateOfModelDataLoader<Component, Models.PhotovoltaicData, Models.ComponentPhotovoltaicData, Models.Component>
        {
            public ComponentOfPhotovoltaicDataDataLoader(IQueryBus queryBus)
              : base(Component.FromModel, queryBus)
            {
            }
        }
    }
}