using System;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public class BackwardManyToManyAssociationsOfModelDataLoader<TAssociationGraphQlObject, TAssociateModel, TAssociationModel>
      : AssociationsOfModelDataLoader<TAssociationGraphQlObject, TAssociateModel, TAssociationModel, Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>>
    {
        public BackwardManyToManyAssociationsOfModelDataLoader(
            Func<TAssociationModel, Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
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