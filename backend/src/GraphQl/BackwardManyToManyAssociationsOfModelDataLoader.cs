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
    public class BackwardManyToManyAssociationsOfModelDataLoader<TAssociationGraphQlObject, TAssociateModel, TAssociationModel>
      : AssociationsOfModelDataLoader<TAssociationGraphQlObject, TAssociateModel, TAssociationModel, Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>>
    {
        public BackwardManyToManyAssociationsOfModelDataLoader(
            Func<TAssociationModel, ValueObjects.Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}