using System;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public class ForwardManyToManyAssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel>
      : AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel, Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>>
    {
        public ForwardManyToManyAssociatesOfModelDataLoader(
            Func<TAssociateModel, Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>.From,
            mapAssociateModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}