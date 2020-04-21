using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetMethodBatch
      : IQuery<IEnumerable<Result<Models.Method, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedMethodIds { get; }

        private GetMethodBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedMethodIds
            )
        {
            TimestampedMethodIds = timestampedMethodIds;
        }

        public static Result<GetMethodBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedMethodIds
            )
        {
            return Result.Ok<GetMethodBatch, Errors>(
                    new GetMethodBatch(
                        timestampedMethodIds
                        )
                    );
        }
    }
}