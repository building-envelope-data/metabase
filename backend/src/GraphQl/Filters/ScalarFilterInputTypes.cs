using HotChocolate.Data.Filters;
using HotChocolate.Types;

namespace Metabase.GraphQl.Filters;

public abstract class ExtendedComparableOperationFilterInputType<T>
    : ComparableOperationFilterInputType<T>
where T : notnull
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        base.Configure(descriptor);
        // descriptor
        //     .Operation(AdditionalFilterOperations.InClosedInterval)
        //     .Type<InputObjectType<ClosedIntervalInput<T>>>();
    }
}

public sealed class StringFilterInputType
    : StringOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"String{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class BooleanFilterInputType
    : BooleanOperationFilterInputType
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Boolean{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class ShortFilterInputType
    : ExtendedComparableOperationFilterInputType<ShortType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Short{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class DateTimeFilterInputType
    : ExtendedComparableOperationFilterInputType<DateTimeType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"DateTime{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class ByteFilterInputType
    : ExtendedComparableOperationFilterInputType<ByteType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Byte{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class UuidFilterInputType
    : ExtendedComparableOperationFilterInputType<UuidType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Uuid{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

// public sealed class LocalDateFilterInputType
//     : ExtendedComparableOperationFilterInputType<HotChocolate.Types.NodaTime.LocalDateType>
// {
//     protected override void Configure(IFilterInputTypeDescriptor descriptor)
//     {
//         descriptor.Name($"LocalDate{GraphQlConstants.FilterInputSuffix}");
//         base.Configure(descriptor);
//     }
// }

public sealed class LongFilterInputType
    : ExtendedComparableOperationFilterInputType<LongType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Long{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

// public sealed class LocalTimeFilterInputType
//     : ExtendedComparableOperationFilterInputType<HotChocolate.Types.NodaTime.LocalTimeType>
// {
//     protected override void Configure(IFilterInputTypeDescriptor descriptor)
//     {
//         descriptor.Name($"LocalTime{GraphQlConstants.FilterInputSuffix}");
//         base.Configure(descriptor);
//     }
// }

public sealed class FloatFilterInputType
    : ExtendedComparableOperationFilterInputType<FloatType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Float{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class TimeSpanFilterInputType
    : ExtendedComparableOperationFilterInputType<TimeSpanType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"TimeSpan{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class IntFilterInputType
    : ExtendedComparableOperationFilterInputType<IntType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Int{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class DecimalFilterInputType
    : ExtendedComparableOperationFilterInputType<DecimalType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Decimal{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class UrlFilterInputType
    : ExtendedComparableOperationFilterInputType<UrlType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Url{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}
