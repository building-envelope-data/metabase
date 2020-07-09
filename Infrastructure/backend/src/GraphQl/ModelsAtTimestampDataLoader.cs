using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Infrastructure.GraphQl
{
    public class ModelsAtTimestampDataLoader<TGraphQlObject, TModel>
      : OurDataLoaderBase<Timestamp, IReadOnlyList<TGraphQlObject>>
    {
        private readonly Func<TModel, Timestamp, TGraphQlObject> _mapModelToGraphQlObject;

        public ModelsAtTimestampDataLoader(
            Func<TModel, Timestamp, TGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TGraphQlObject>>>> FetchAsync(
            IReadOnlyList<Timestamp> timestamps,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetModelsAtTimestamps<TModel>.From(timestamps)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetModelsAtTimestamps<TModel>,
                  IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResultsX<TGraphQlObject>(
                timestamps.Zip(results, (timestamp, result) =>
                  result.Map(associateResults =>
                    associateResults.Select(associateResult =>
                      associateResult.Map(m => _mapModelToGraphQlObject(m, timestamp))
                      )
                    )
                  )
                );
        }
    }
}