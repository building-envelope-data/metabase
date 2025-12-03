using System;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed record OpenEndedDateTimeRange(
    OffsetDateTime From,
    OffsetDateTime Until
);