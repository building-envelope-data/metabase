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
    public sealed class OpticalDataIkdbOfComponentDataLoader
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<OpticalDataIkdb>>
    {
        public OpticalDataIkdbOfComponentDataLoader(
            IQueryBus queryBus
            )
          : base(queryBus)
        {
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<OpticalDataIkdb>>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetOpticalDataIkdbOfComponents.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetOpticalDataIkdbOfComponents,
                  IEnumerable<Result<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResultsX<OpticalDataIkdb>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(associateResults =>
                    associateResults.Select(associateResult =>
                      associateResult.Map(model => OpticalDataIkdb.FromModel(model, timestampedId.Timestamp))
                      )
                    )
                  )
                );
        }
    }
}
