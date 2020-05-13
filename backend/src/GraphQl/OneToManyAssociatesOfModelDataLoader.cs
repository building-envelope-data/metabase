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
    public class OneToManyAssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociateModel>
      : AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociateModel, TAssociateModel, Queries.GetOneToManyAssociatesOfModels<TModel, TAssociateModel>>
    {
        public OneToManyAssociatesOfModelDataLoader(
            Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetOneToManyAssociatesOfModels<TModel, TAssociateModel>.From,
            mapAssociateModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}