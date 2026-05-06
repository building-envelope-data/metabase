using System;
using System.Text;

namespace Metabase.GraphQl;

public static class PaginationHelpers
{
    public static string ConstructCursor(Guid id)
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                id.ToString("D")
            )
        );
    }

    public static string ConstructCursor(Guid id1, Guid id2)
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes(
                $"{id1:D}:{id2:D}"
            )
        );
    }
}