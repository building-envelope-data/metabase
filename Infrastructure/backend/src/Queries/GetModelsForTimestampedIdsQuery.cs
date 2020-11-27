using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public sealed class GetModelsForTimestampedIdsQuery<M>
      : IQuery<IEnumerable<Result<M, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        private GetModelsForTimestampedIdsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetModelsForTimestampedIdsQuery<M>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetModelsForTimestampedIdsQuery<M>, Errors>(
                    new GetModelsForTimestampedIdsQuery<M>(
                        timestampedIds
                        )
                    );
        }
    }
}