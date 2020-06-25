using System;
using Icon.Infrastructure.Query;

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