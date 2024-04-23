using System;

namespace Metabase.GraphQl.DataX;

public sealed class OpenEndedDateTimeRange
{
    public OpenEndedDateTimeRange(
        DateTime from,
        DateTime until
    )
    {
        From = from;
        Until = until;
    }

    public DateTime From { get; }
    public DateTime Until { get; }
}