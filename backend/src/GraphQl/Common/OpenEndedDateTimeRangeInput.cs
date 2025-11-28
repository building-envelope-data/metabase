using System;
using NpgsqlTypes;

namespace Metabase.GraphQl.Common;

public sealed record OpenEndedDateTimeRangeInput(
    DateTime? From,
    DateTime? To
)
{
    public NpgsqlRange<DateTime> ToDomainModel()
    {
        return new NpgsqlRange<DateTime>(
            From.GetValueOrDefault(), true, From is null,
            To.GetValueOrDefault(), true, To is null
        );
    }
};