using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public class ForwardManyToManyAssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel>
      : AssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>>
    {
        public ForwardManyToManyAssociationsOfModelDataLoader(
            Func<TAssociationModel, Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>.From,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}