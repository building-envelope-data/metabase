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
    public class BackwardManyToManyAssociatesOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel>
      : AssociatesOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel, Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>>
    {
        public BackwardManyToManyAssociatesOfModelDataLoader(
            Func<TModel, ValueObjects.Timestamp, TGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>.From,
            mapModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}