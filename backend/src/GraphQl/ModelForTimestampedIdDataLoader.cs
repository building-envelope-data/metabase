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
    public class ModelForTimestampedIdDataLoader<T, M>
      : OurDataLoaderBase<ValueObjects.TimestampedId, T>
    {
        private readonly Func<M, ValueObjects.Timestamp, T> _mapModelToGraphQlObject;

        public ModelForTimestampedIdDataLoader(
            Func<M, ValueObjects.Timestamp, T> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<T>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken)
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetModelsForTimestampedIds<M>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetModelsForTimestampedIds<M>,
                  IEnumerable<Result<M, Errors>>
               >(query);
            return ResultHelpers.ToDataLoaderResults<T>(
                timestampedIds.Zip(
                  results,
                  (timestampedId, result) =>
                    result.Map(m => _mapModelToGraphQlObject(m, timestampedId.Timestamp))
                  )
                );
        }
    }
}