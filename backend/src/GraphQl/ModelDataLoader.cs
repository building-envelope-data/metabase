using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using HotChocolate;
using Icon.Infrastructure.Query;
using System.Linq;
using HotChocolate.Resolvers;
using CancellationToken = System.Threading.CancellationToken;
using CSharpFunctionalExtensions;
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
                  Queries.GetModels<TModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetModels<TModel>,
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