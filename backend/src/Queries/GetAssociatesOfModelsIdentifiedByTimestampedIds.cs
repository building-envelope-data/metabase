using ValueObjects = Icon.ValueObjects;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System.Linq;

namespace Icon.Queries
{
    public sealed class GetAssociatesOfModelsIdentifiedByTimestampedIds<M, N>
      : IQuery<IEnumerable<Result<IEnumerable<Result<N, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedModelIds { get; }

        private GetAssociatesOfModelsIdentifiedByTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            TimestampedModelIds = timestampedModelIds;
        }

        public static Result<GetAssociatesOfModelsIdentifiedByTimestampedIds<M, N>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetAssociatesOfModelsIdentifiedByTimestampedIds<M, N>, Errors>(
                new GetAssociatesOfModelsIdentifiedByTimestampedIds<M, N>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}