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
    public sealed class HasOpticalDataForComponentDataLoader
      : OurDataLoaderBase<ValueObjects.TimestampedId, bool>
    {
        public HasOpticalDataForComponentDataLoader(
            IQueryBus queryBus
            )
          : base(queryBus)
        {
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<bool>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.HasOpticalDataForComponents.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.HasOpticalDataForComponents,
                  IEnumerable<Result<bool, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResults<bool>(results);
        }
    }
}