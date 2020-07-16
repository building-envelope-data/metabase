using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class WhichDatabasesHaveDataForComponents<TDataModel>
      : QueryDataOfComponentsFromDatabases<IEnumerable<Result<Models.Database, Errors>>>
    {
        private WhichDatabasesHaveDataForComponents(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }

        public static Result<WhichDatabasesHaveDataForComponents<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<WhichDatabasesHaveDataForComponents<TDataModel>, Errors>(
                new WhichDatabasesHaveDataForComponents<TDataModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}