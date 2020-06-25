using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;

namespace Icon.GraphQl
{
    public abstract class AssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, TQuery>
      : AssociatesOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, TAssociationModel, TQuery>
      where TQuery : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        public AssociationsOfModelDataLoader(
            Func<IReadOnlyList<ValueObjects.TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TAssociationModel, ValueObjects.Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
            newQuery,
            mapAssociationModelToGraphQlObject,
            queryBus
            )
        {
        }
    }
}