using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using HotChocolate;
using Icon.Infrastructure.Query;
using System.Linq;
using HotChocolate.Resolvers;
using CancellationToken = System.Threading.CancellationToken;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;

namespace Icon.GraphQl
{
    public class BackwardAssociationsOfModelIdentifiedByTimestampedIdDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel>
      : AssociationsOfModelIdentifiedByTimestampedIdDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, Queries.GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>>
    {
        public BackwardAssociationsOfModelIdentifiedByTimestampedIdDataLoader(
            Func<TAssociationModel, ValueObjects.Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetBackwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}