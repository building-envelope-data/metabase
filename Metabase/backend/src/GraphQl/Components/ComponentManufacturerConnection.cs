using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Types.Pagination;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerConnection
        : Connection
    {
        public ComponentManufacturerConnection(
            IReadOnlyCollection<ComponentManufacturerEdge> edges,
            ConnectionPageInfo info,
            Func<CancellationToken, ValueTask<int>> getTotalCount)
            : base(edges, info, getTotalCount)
        {
            Edges = edges;
        }

        public new IReadOnlyCollection<ComponentManufacturerEdge> Edges { get; }
    }
}