using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class QueryDataArrayOfComponentsFromDatabases<TDataModel>
      : QueryDataOfComponentsFromDatabases<IEnumerable<Result<TDataModel, Errors>>>
    {
        private QueryDataArrayOfComponentsFromDatabases(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }

        public static Result<QueryDataArrayOfComponentsFromDatabases<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Ok<QueryDataArrayOfComponentsFromDatabases<TDataModel>, Errors>(
                    new QueryDataArrayOfComponentsFromDatabases<TDataModel>(
                        timestampedIds
                        )
                    );
        }
    }
}