using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Database.GraphQl
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
                  Queries.HasDataForComponentsQuery<TDataModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.HasDataForComponentsQuery<TDataModel>,
                  IEnumerable<Result<bool, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResults<bool>(results);
        }
    }
}