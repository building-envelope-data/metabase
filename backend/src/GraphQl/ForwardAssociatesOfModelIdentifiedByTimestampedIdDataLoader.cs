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
    public class ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel>
      : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel, Queries.GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>>
    {
        public ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader(
            Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociationModel, TAssociateModel>.From,
            mapAssociateModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}