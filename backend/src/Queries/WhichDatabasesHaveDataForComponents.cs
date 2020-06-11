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