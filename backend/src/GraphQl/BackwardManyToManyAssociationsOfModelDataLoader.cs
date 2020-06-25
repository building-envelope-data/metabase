using System;
using Icon.Infrastructure.Query;

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