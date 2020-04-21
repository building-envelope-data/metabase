using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetStandardBatch
      : IQuery<IEnumerable<Result<Models.Standard, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedStandardIds { get; }

        private GetStandardBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedStandardIds
            )
        {
            TimestampedStandardIds = timestampedStandardIds;
        }

        public static Result<GetStandardBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedStandardIds
            )
        {
            return Result.Ok<GetStandardBatch, Errors>(
                    new GetStandardBatch(
                        timestampedStandardIds
                        )
                    );
        }
    }
}