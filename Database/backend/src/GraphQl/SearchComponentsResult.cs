using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
{
    public sealed class SearchComponentsResult
    {
        public IReadOnlyList<SearchComponentResult> Components { get; }
        public Timestamp RequestTimestamp { get; }

        public SearchComponentsResult(
            IReadOnlyList<SearchComponentResult> components,
            Timestamp requestTimestamp
            )
        {
            Components = components;
            RequestTimestamp = requestTimestamp;
        }
    }
}