using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public abstract class SearchComponentsResult<TSearchComponentResult>
    {
        public IReadOnlyList<TSearchComponentResult> Components { get; }
        public Timestamp RequestTimestamp { get; }

        protected SearchComponentsResult(
            IReadOnlyList<TSearchComponentResult> components,
            Timestamp requestTimestamp
            )
        {
            Components = components;
            RequestTimestamp = requestTimestamp;
        }
    }
}
