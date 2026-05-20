using System;
using Metabase.Extensions;

namespace Metabase.GraphQl;

public static class PaginationHelpers
{
    public static string ConstructCursor(Guid id)
    {
        return id.ToString("D").Base64Encode();
    }

    public static string ConstructCursor(Guid id1, Guid id2)
    {
        return $"{id1:D}:{id2:D}".Base64Encode();
    }
}