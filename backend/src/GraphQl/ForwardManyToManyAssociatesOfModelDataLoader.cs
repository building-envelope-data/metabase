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
    public class ForwardManyToManyAssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel>
      : AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel, Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>>
    {
        public ForwardManyToManyAssociatesOfModelDataLoader(
            Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>.From,
            mapAssociateModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}