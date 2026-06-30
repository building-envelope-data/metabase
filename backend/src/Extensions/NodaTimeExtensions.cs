using System.Diagnostics.Contracts;
using NodaTime;

namespace Metabase.Extensions;

public static class NodaTimeExtensions
{
    [Pure]
    public static OffsetDateTime GetUtcNow(this IClock clock)
    {
        return clock.GetCurrentInstant().WithOffset(Offset.Zero);
    }

    [Pure]
    public static int CompareTo(this OffsetDateTime current, OffsetDateTime other)
    {
        return OffsetDateTime.Comparer.Instant.Compare(current, other);
    }

    extension(OffsetDateTime)
    {
        [Pure]
        public static bool operator >(OffsetDateTime x, OffsetDateTime y)
        {
            return OffsetDateTime.Comparer.Instant.Compare(x, y) > 0;
        }

        [Pure]
        public static bool operator <(OffsetDateTime x, OffsetDateTime y)
        {
            return OffsetDateTime.Comparer.Instant.Compare(x, y) < 0;
        }
    }
}