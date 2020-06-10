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
    public abstract class QueryDatabasesDataLoader<TQuery, TResultModel, TResultGraphQlObject>
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<TResultGraphQlObject>>
      where TQuery : IQuery<IEnumerable<Result<IEnumerable<Result<TResultModel, Errors>>, Errors>>>
    {
        private Func<IReadOnlyCollection<ValueObjects.TimestampedId>, Result<TQuery, Errors>> _newQuery;
        private Func<TResultModel, ValueObjects.Timestamp, TResultGraphQlObject> _mapModelToGraphQlObject;

        public QueryDatabasesDataLoader(
            Func<IReadOnlyCollection<ValueObjects.TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TResultModel, ValueObjects.Timestamp, TResultGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _newQuery = newQuery;
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TResultGraphQlObject>>>> FetchAsync(
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
                  IEnumerable<Result<IEnumerable<Result<TResultModel, Errors>>, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResultsX<TResultGraphQlObject>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(modelResults =>
                    modelResults.Select(modelResult =>
                      modelResult.Map(model =>
                        _mapModelToGraphQlObject(model, timestampedId.Timestamp)
                        )
                      )
                    )
                  )
                );
        }
    }
}
