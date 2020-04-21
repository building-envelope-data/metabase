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
    public class AssociatesOfModelIdentifiedByTimestampedIdDataLoader<T, M, N>
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<T>>
    {
        private readonly Func<N, ValueObjects.Timestamp, T> _mapAssociateModelToGraphQlObject;

        public AssociatesOfModelIdentifiedByTimestampedIdDataLoader(
            Func<N, ValueObjects.Timestamp, T> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapAssociateModelToGraphQlObject = mapAssociateModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<T>>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<M, N>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<M, N>,
                  IEnumerable<Result<IEnumerable<Result<N, Errors>>, Errors>>
               >(query);
            return ResultHelpers.ToDataLoaderResultsX<T>(
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