using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class HasDataForComponentDataLoader<TDataModel>
      : OurDataLoaderBase<TimestampedId, bool>
    {
        public HasDataForComponentDataLoader(
            IQueryBus queryBus
            )
          : base(queryBus)
        {
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<bool>>> FetchAsync(
            IReadOnlyList<TimestampedId> timestampedIds,
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