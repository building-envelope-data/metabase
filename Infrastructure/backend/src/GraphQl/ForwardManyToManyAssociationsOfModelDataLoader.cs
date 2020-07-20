using System;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public class ForwardManyToManyAssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel>
      : AssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, Queries.GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>>
    {
        public ForwardManyToManyAssociationsOfModelDataLoader(
            Func<TAssociationModel, Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}