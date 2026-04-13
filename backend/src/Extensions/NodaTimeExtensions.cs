using NodaTime;

namespace Metabase.Extensions;

public static class NodaTimeExtensions
{
    extension(OffsetDateTime)
    {
        public static OffsetDateTime UtcNow =>
            SystemClock.Instance
            .GetCurrentInstant()
            .WithOffset(Offset.Zero);

        public static bool operator >(OffsetDateTime x, OffsetDateTime y)
        {
            return OffsetDateTime.Comparer.Instant.Compare(x, y) > 0;
        }

        public static bool operator <(OffsetDateTime x, OffsetDateTime y)
        {
            return OffsetDateTime.Comparer.Instant.Compare(x, y) < 0;
        }
    }
}