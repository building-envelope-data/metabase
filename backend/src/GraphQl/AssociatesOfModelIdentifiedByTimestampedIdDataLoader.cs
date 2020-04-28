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
    public class AssociatesOfModelIdentifiedByTimestampedIdDataLoader<TAssociateGraphQlObject, TModel, TAssociateModel>
      : OurDataLoaderBase<ValueObjects.TimestampedId, IReadOnlyList<TAssociateGraphQlObject>>
    {
        private readonly Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> _mapAssociateModelToGraphQlObject;

        public AssociatesOfModelIdentifiedByTimestampedIdDataLoader(
            Func<TAssociateModel, ValueObjects.Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapAssociateModelToGraphQlObject = mapAssociateModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TAssociateGraphQlObject>>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociateModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociateModel>,
                  IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
               >(query);
            return ResultHelpers.ToDataLoaderResultsX<TAssociateGraphQlObject>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(associateResults =>
                    associateResults.Select(associateResult =>
                      associateResult.Map(n => _mapAssociateModelToGraphQlObject(n, timestampedId.Timestamp))
                      )
                    )
                  )
                );
        }
    }
}