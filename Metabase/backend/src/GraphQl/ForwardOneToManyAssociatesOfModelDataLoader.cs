using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public class ForwardOneToManyAssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel>
      : AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociateModel, TAssociateModel, Queries.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>>
    {
        public ForwardOneToManyAssociatesOfModelDataLoader(
            Func<TAssociateModel, Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
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