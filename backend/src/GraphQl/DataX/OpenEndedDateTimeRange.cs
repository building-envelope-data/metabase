using System;

namespace Metabase.GraphQl.DataX;

public sealed record OpenEndedDateTimeRange(
    DateTime From,
    DateTime Until
);