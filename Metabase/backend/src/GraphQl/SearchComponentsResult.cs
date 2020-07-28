using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class SearchComponentsResult
      : Infrastructure.GraphQl.SearchComponentsResult<SearchComponentResult>
    {
        public SearchComponentsResult(
            IReadOnlyList<SearchComponentResult> components,
            Timestamp requestTimestamp
            )
          : base(
              components,
              requestTimestamp
              )
        {
        }
    }
}