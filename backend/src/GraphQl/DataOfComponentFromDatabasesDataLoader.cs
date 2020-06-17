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
    public abstract class DataOfComponentFromDatabasesDataLoader<TQuery, TDataModel, TDataGraphQlObject>
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<TDataGraphQlObject>>
      where TQuery : Queries.QueryDataOfComponentsFromDatabases<IEnumerable<Result<TDataModel, Errors>>>
    {
        private Func<IReadOnlyCollection<ValueObjects.TimestampedId>, Result<TQuery, Errors>> _newQuery;
        private Func<TDataModel, ValueObjects.Timestamp, TDataGraphQlObject> _mapModelToGraphQlObject;

        public DataOfComponentFromDatabasesDataLoader(
            Func<IReadOnlyCollection<ValueObjects.TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TDataModel, ValueObjects.Timestamp, TDataGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _newQuery = newQuery;
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TDataGraphQlObject>>>> FetchAsync(
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
                  IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResultsX<TDataGraphQlObject>(
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