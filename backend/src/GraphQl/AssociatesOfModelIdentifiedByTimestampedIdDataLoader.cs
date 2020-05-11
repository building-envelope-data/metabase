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
    public abstract class AssociatesOfModelIdentifiedByTimestampedIdDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel, TQuery>
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<TAssociateGraphQlObject>>
      where TQuery : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private readonly Func<IReadOnlyList<ValueObjects.TimestampedId>, Result<TQuery, Errors>> _newQuery;
        private readonly Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> _mapAssociateModelToGraphQlObject;

        public AssociatesOfModelIdentifiedByTimestampedIdDataLoader(
            Func<IReadOnlyList<ValueObjects.TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _newQuery = newQuery;
            _mapAssociateModelToGraphQlObject = mapAssociateModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TAssociateGraphQlObject>>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  _newQuery(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  TQuery,
                  IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResultsX<TAssociateGraphQlObject>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(associateResults =>
                    associateResults.Select(associateResult =>
                      associateResult.Map(n => _mapAssociateModelToGraphQlObject(n, timestampedId.Timestamp))
                      )
                    )
                  )
                );
        }
    }
}