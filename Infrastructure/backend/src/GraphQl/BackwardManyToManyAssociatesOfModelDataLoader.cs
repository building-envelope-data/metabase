using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public class BackwardManyToManyAssociatesOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel>
      : AssociatesOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel, Queries.GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>>
    {
        public BackwardManyToManyAssociatesOfModelDataLoader(
            Func<TModel, Timestamp, TGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>.From,
            mapModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}