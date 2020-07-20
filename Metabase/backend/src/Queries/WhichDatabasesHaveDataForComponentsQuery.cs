using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class WhichDatabasesHaveDataForComponentsQuery<TDataModel>
      : QueryDataOfComponentsFromDatabasesQuery<IEnumerable<Result<Models.Database, Errors>>>
    {
        private WhichDatabasesHaveDataForComponentsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }

        public static Result<WhichDatabasesHaveDataForComponentsQuery<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<WhichDatabasesHaveDataForComponentsQuery<TDataModel>, Errors>(
                new WhichDatabasesHaveDataForComponentsQuery<TDataModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}