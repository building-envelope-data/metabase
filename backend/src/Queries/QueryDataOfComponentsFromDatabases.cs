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
    public abstract class QueryDataOfComponentsFromDatabases<TData>
      : IQuery<IEnumerable<Result<TData, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        protected QueryDataOfComponentsFromDatabases(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}