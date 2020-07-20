using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public abstract class DataOfComponentFromDatabasesDataLoader<TQuery, TDataModel, TDataGraphQlObject>
      : OurDataLoaderBase<TimestampedId, IReadOnlyList<TDataGraphQlObject>>
      where TQuery : Queries.QueryDataOfComponentsFromDatabasesQuery<IEnumerable<Result<TDataModel, Errors>>>
    {
        private readonly Func<IReadOnlyCollection<TimestampedId>, Result<TQuery, Errors>> _newQuery;
        private readonly Func<TDataModel, Timestamp, TDataGraphQlObject> _mapModelToGraphQlObject;

        protected DataOfComponentFromDatabasesDataLoader(
            Func<IReadOnlyCollection<TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TDataModel, Timestamp, TDataGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _newQuery = newQuery;
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TDataGraphQlObject>>>> FetchAsync(
            IReadOnlyList<TimestampedId> timestampedIds,
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