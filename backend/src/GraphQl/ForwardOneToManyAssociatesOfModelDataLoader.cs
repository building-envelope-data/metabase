using System;
using Icon.Infrastructure.Queries;

namespace Icon.GraphQl
{
    public class ForwardOneToManyAssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel>
      : AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociateModel, TAssociateModel, Queries.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>>
    {
        public ForwardOneToManyAssociatesOfModelDataLoader(
            Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>.From,
            mapAssociateModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}