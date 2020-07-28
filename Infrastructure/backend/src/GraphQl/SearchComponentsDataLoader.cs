using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Infrastructure.GraphQl;
using CancellationToken = System.Threading.CancellationToken;

namespace Infrastructure.GraphQl
{
    public sealed class SearchComponentsDataLoader<TVariable, TResultModel, TResultGraphQlObject>
      : OurDataLoaderBase<(ValueObjects.Proposition<TVariable>, ValueObjects.Timestamp), TResultGraphQlObject>
    {
        private readonly Func<TResultModel, ValueObjects.Timestamp, TResultGraphQlObject> _mapModelToGraphQlObject;

        public SearchComponentsDataLoader(
            Queries.IQueryBus queryBus,
            Func<TResultModel, ValueObjects.Timestamp, TResultGraphQlObject> mapModelToGraphQlObject
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<TResultGraphQlObject>>> FetchAsync(
            IReadOnlyList<(ValueObjects.Proposition<TVariable>, ValueObjects.Timestamp)> propositionsAndTimestamps,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.SearchComponentsQuery<TVariable, TResultModel>.From(propositionsAndTimestamps)
                  );
            var results =
              await QueryBus.Send<
                  Queries.SearchComponentsQuery<TVariable, TResultModel>,
                  IEnumerable<Result<TResultModel, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResults<TResultGraphQlObject>(
                propositionsAndTimestamps.Zip(results, (propositionAndTimestamp, result) =>
                  result.Map(searchComponentsResult =>
                    _mapModelToGraphQlObject(
                      searchComponentsResult,
                      propositionAndTimestamp.Item2
                      )
                    )
                  )
                );
        }
    }
}
