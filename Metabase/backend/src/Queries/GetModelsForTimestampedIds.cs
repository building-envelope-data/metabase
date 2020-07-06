using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetModelsForTimestampedIds<M>
      : IQuery<IEnumerable<Result<M, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        private GetModelsForTimestampedIds(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetModelsForTimestampedIds<M>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetModelsForTimestampedIds<M>, Errors>(
                    new GetModelsForTimestampedIds<M>(
                        timestampedIds
                        )
                    );
        }
    }
}