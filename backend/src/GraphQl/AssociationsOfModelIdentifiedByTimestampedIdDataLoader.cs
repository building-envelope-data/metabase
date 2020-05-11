using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using HotChocolate;
using Icon.Infrastructure.Query;
using System.Linq;
using HotChocolate.Resolvers;
using CancellationToken = System.Threading.CancellationToken;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;

namespace Icon.GraphQl
{
    public abstract class AssociationsOfModelIdentifiedByTimestampedIdDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, TQuery>
      : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<TAssociationGraphQlObject, TModel, TAssociationModel, TAssociationModel, TQuery>
      where TQuery : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        public AssociationsOfModelIdentifiedByTimestampedIdDataLoader(
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