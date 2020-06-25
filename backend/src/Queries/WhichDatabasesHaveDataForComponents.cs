using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class WhichDatabasesHaveDataForComponents<TDataModel>
      : QueryDataOfComponentsFromDatabases<IEnumerable<Result<Models.Database, Errors>>>
    {
        private WhichDatabasesHaveDataForComponents(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }

        public static Result<WhichDatabasesHaveDataForComponents<TDataModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<WhichDatabasesHaveDataForComponents<TDataModel>, Errors>(
                new WhichDatabasesHaveDataForComponents<TDataModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}