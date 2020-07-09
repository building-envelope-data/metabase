using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.GraphQl
{
    public abstract class AssociationsOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, TQuery>
      : AssociatesOfModelDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, TAssociationModel, TQuery>
      where TQuery : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        protected AssociationsOfModelDataLoader(
            Func<IReadOnlyList<TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TAssociationModel, Timestamp, TAssociationGraphQlObject> mapAssociationModelToGraphQlObject,
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