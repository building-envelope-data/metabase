using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class QueryDataArrayOfComponentsFromDatabases<TDataModel>
      : QueryDataOfComponentsFromDatabases<IEnumerable<Result<TDataModel, Errors>>>
    {
        private QueryDataArrayOfComponentsFromDatabases(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }

        public static Result<QueryDataArrayOfComponentsFromDatabases<TDataModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
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