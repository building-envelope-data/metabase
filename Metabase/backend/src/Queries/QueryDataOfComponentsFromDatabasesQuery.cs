using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public abstract class QueryDataOfComponentsFromDatabasesQuery<TData>
      : IQuery<IEnumerable<Result<TData, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        protected QueryDataOfComponentsFromDatabasesQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}