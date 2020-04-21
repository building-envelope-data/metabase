using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetUserBatch
      : IQuery<IEnumerable<Result<Models.User, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedUserIds { get; }

        private GetUserBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedUserIds
            )
        {
            TimestampedUserIds = timestampedUserIds;
        }

        public static Result<GetUserBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedUserIds
            )
        {
            return Result.Ok<GetUserBatch, Errors>(
                    new GetUserBatch(
                        timestampedUserIds
                        )
                    );
        }
    }
}