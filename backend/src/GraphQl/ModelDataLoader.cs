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
    public class ModelDataLoader<TGraphQlObject, TModel>
      : OurDataLoaderBase<ValueObjects.TimestampedId, TGraphQlObject>
    {
        private readonly Func<TModel, ValueObjects.Timestamp, TGraphQlObject> _mapModelToGraphQlObject;

        public ModelDataLoader(
            Func<TModel, ValueObjects.Timestamp, TGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<TGraphQlObject>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken)
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetModelsForTimestampedIds<TModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetModelsForTimestampedIds<TModel>,
                  IEnumerable<Result<TModel, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResults<TGraphQlObject>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(m => _mapModelToGraphQlObject(m, timestampedId.Timestamp))
                  )
                );
        }
    }
}