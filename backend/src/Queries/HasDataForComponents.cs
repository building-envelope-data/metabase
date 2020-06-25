using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public sealed class HasDataForComponents<TDataModel>
      : IQuery<IEnumerable<Result<bool, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        private HasDataForComponents(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<HasDataForComponents<TDataModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<HasDataForComponents<TDataModel>, Errors>(
                new HasDataForComponents<TDataModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}