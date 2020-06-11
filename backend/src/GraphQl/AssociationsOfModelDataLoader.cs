using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Icon.Infrastructure.Query;
using CancellationToken = System.Threading.CancellationToken;
using IError = HotChocolate.IError;

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