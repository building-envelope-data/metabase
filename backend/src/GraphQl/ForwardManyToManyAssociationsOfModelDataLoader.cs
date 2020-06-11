using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Icon.Infrastructure.Query;
using CancellationToken = System.Threading.CancellationToken;
using IError = HotChocolate.IError;

namespace Icon.GraphQl
{
    public class ForwardManyToManyAssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel>
      : AssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>>
    {
        public ForwardManyToManyAssociationsOfModelDataLoader(
            Func<TAssociationModel, ValueObjects.Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}