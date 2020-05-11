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
    public class ForwardAssociationsOfModelIdentifiedByTimestampedIdDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel>
      : AssociationsOfModelIdentifiedByTimestampedIdDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, Queries.GetForwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>>
    {
        public ForwardAssociationsOfModelIdentifiedByTimestampedIdDataLoader(
            Func<TAssociationModel, ValueObjects.Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardAssociationsOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}