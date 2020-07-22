using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public class ForwardOneToManyAssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel>
      : AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociateModel, TAssociateModel, Queries.GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>>
    {
        public ForwardOneToManyAssociatesOfModelDataLoader(
            Func<TAssociateModel, Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>.From,
            mapAssociateModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}