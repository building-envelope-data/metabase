using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Queries;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.GraphQl
{
    public sealed class HasDataForComponentDataLoader<TDataModel>
      : OurDataLoaderBase<ValueObjects.TimestampedId, bool>
    {
        public HasDataForComponentDataLoader(
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
                  Queries.HasDataForComponents<TDataModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.HasDataForComponents<TDataModel>,
                  IEnumerable<Result<bool, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResults<bool>(results);
        }
    }
}