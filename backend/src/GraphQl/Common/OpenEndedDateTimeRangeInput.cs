using HotChocolate;
using HotChocolate.Types;
using NodaTime;
using NpgsqlTypes;

namespace Metabase.GraphQl.Common;

public sealed record OpenEndedDateTimeRangeInput(
    [GraphQLType<DateTimeType>] OffsetDateTime? From,
    [GraphQLType<DateTimeType>] OffsetDateTime? To
)
{
    public NpgsqlRange<OffsetDateTime> ToDomainModel()
    {
        return new(
            From.GetValueOrDefault(), true, From is null,
            To.GetValueOrDefault(), true, To is null
        );
    }
};