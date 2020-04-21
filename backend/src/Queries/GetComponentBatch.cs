using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetComponentBatch
      : IQuery<IEnumerable<Result<Models.Component, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedComponentIds { get; }

        private GetComponentBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedComponentIds
            )
        {
            TimestampedComponentIds = timestampedComponentIds;
        }

        public static Result<GetComponentBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedComponentIds
            )
        {
            return Result.Ok<GetComponentBatch, Errors>(
                    new GetComponentBatch(
                        timestampedComponentIds
                        )
                    );
        }
    }
}