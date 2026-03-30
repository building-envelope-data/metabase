using HotChocolate.Types;
using HotChocolate.Types.NodaTime;
using NodaTime;
using NpgsqlTypes;
using DateTimeType = HotChocolate.Types.NodaTime.DateTimeType;

namespace Metabase.GraphQl.Common;

public sealed class OpenEndedDateTimeRangeType
    : ObjectType<NpgsqlRange<OffsetDateTime>>
{
    protected override void Configure(
        IObjectTypeDescriptor<NpgsqlRange<OffsetDateTime>> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();

        const string SuffixedName = nameof(OpenEndedDateTimeRangeType);
        descriptor.Name(SuffixedName[..^"Type".Length]);

        descriptor
            .Field("from")
            .Type<DateTimeType>()
            .Cost(0)
            .Resolve(context =>
                {
                    var range = context.Parent<NpgsqlRange<OffsetDateTime>>();
                    return range.LowerBoundInfinite
                        ? null
                        : range.LowerBound;
                }
            );

        descriptor
            .Field("to")
            .Type<DateTimeType>()
            .Cost(0)
            .Resolve(context =>
                {
                    var range = context.Parent<NpgsqlRange<OffsetDateTime>>();
                    return range.UpperBoundInfinite
                        ? null
                        : range.UpperBound;
                }
            );
    }
}