using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class QueryDataArrayOfComponentsFromDatabasesQuery<TDataModel>
      : QueryDataOfComponentsFromDatabasesQuery<IEnumerable<Result<TDataModel, Errors>>>
    {
        private QueryDataArrayOfComponentsFromDatabasesQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }

        public static Result<QueryDataArrayOfComponentsFromDatabasesQuery<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<QueryDataArrayOfComponentsFromDatabasesQuery<TDataModel>, Errors>(
                    new QueryDataArrayOfComponentsFromDatabasesQuery<TDataModel>(
                        timestampedIds
                        )
                    );
        }
    }
}