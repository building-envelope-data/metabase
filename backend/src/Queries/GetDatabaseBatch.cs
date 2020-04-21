using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetDatabaseBatch
      : IQuery<IEnumerable<Result<Models.Database, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedDatabaseIds { get; }

        private GetDatabaseBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedDatabaseIds
            )
        {
            TimestampedDatabaseIds = timestampedDatabaseIds;
        }

        public static Result<GetDatabaseBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedDatabaseIds
            )
        {
            return Result.Ok<GetDatabaseBatch, Errors>(
                    new GetDatabaseBatch(
                        timestampedDatabaseIds
                        )
                    );
        }
    }
}