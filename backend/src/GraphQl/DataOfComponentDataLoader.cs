using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Icon.Infrastructure.Query;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.GraphQl
{
    public abstract class DataOfComponentDataLoader<TDataModel, TDataGraphQlObject>
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<TDataGraphQlObject>>
    {
        private Func<TDataModel, ValueObjects.Timestamp, TDataGraphQlObject> _mapModelToGraphQlObject;

        public DataOfComponentDataLoader(
            Func<TDataModel, ValueObjects.Timestamp, TDataGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TDataGraphQlObject>>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetDataOfComponents<TDataModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetDataOfComponents<TDataModel>,
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