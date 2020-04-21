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
    public class ModelsAtTimestampDataLoader<T, M>
      : OurDataLoaderBase<ValueObjects.Timestamp, IReadOnlyList<T>>
    {
        private readonly Func<M, ValueObjects.Timestamp, T> _mapModelToGraphQlObject;

        public ModelsAtTimestampDataLoader(
            Func<M, ValueObjects.Timestamp, T> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<T>>>> FetchAsync(
            IReadOnlyList<ValueObjects.Timestamp> timestamps,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetModelsAtTimestamps<M>.From(timestamps)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetModelsAtTimestamps<M>,
                  IEnumerable<Result<IEnumerable<Result<M, Errors>>, Errors>>
               >(query);
            return ResultHelpers.ToDataLoaderResultsX<T>(
                timestamps.Zip(results, (timestamp, result) =>
                  result.Map(associateResults =>
                    associateResults.Select(associateResult =>
                      associateResult.Map(m => _mapModelToGraphQlObject(m, timestamp))
                      )
                    )
                  )
                );
        }
    }
}