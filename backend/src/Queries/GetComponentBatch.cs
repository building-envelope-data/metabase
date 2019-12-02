using System.Collections.Generic;
using Icon.Infrastructure.Query;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System.Linq;

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