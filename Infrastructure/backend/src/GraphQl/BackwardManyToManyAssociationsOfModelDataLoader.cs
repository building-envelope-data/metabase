using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public class BackwardManyToManyAssociationsOfModelDataLoader<TAssociationGraphQlObject, TAssociateModel, TAssociationModel>
      : AssociationsOfModelDataLoader<TAssociationGraphQlObject, TAssociateModel, TAssociationModel, Queries.GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>>
    {
        public BackwardManyToManyAssociationsOfModelDataLoader(
            Func<TAssociationModel, Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}