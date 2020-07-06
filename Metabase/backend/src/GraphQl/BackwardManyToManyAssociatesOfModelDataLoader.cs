using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public class BackwardManyToManyAssociatesOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel>
      : AssociatesOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel, Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>>
    {
        public BackwardManyToManyAssociatesOfModelDataLoader(
            Func<TModel, Timestamp, TGraphQlObject> mapModelToGraphQlObject,
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