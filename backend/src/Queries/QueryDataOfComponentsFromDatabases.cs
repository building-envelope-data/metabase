using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Queries;

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