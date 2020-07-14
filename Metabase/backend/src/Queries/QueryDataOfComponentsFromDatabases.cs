using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public abstract class QueryDataOfComponentsFromDatabases<TData>
      : IQuery<IEnumerable<Result<TData, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        protected QueryDataOfComponentsFromDatabases(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}