using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Queries;

namespace Icon.Queries
{
    public sealed class GetModelsAtTimestamps<M>
      : IQuery<IEnumerable<Result<IEnumerable<Result<M, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.Timestamp> Timestamps { get; }

        private GetModelsAtTimestamps(
            IReadOnlyCollection<ValueObjects.Timestamp> timestamps
            )
        {
            Timestamps = timestamps;
        }

        public static Result<GetModelsAtTimestamps<M>, Errors> From(
            IReadOnlyCollection<ValueObjects.Timestamp> timestamps
            )
        {
            return Result.Ok<GetModelsAtTimestamps<M>, Errors>(
                    new GetModelsAtTimestamps<M>(
                        timestamps
                        )
                    );
        }
    }
}