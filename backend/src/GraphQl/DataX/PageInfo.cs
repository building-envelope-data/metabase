using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class PageInfo
    {
        public PageInfo(
        bool hasNextPage,
        bool hasPreviousPage,
        string startCursor,
        string endCursor,
        uint count
        )
        {
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
            StartCursor = startCursor;
            EndCursor = endCursor;
            Count = count;
        }

        public bool HasNextPage { get; }
        public bool HasPreviousPage { get; }
        public string StartCursor { get; }
        public string EndCursor { get; }
        public uint Count { get; }
    }
}
