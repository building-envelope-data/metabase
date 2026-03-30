using HotChocolate.Data.Filters;
using HotChocolate.Types;
using DateTimeType = HotChocolate.Types.NodaTime.DateTimeType;
using DurationType = HotChocolate.Types.NodaTime.DurationType;
using LocalDateTimeType = HotChocolate.Types.NodaTime.LocalDateTimeType;
using LocalDateType = HotChocolate.Types.NodaTime.LocalDateType;
using LocalTimeType = HotChocolate.Types.NodaTime.LocalTimeType;

namespace Metabase.GraphQl.Filters;

// https://github.com/ChilliCream/graphql-platform/blob/main/src/HotChocolate/Data/src/Data/Filters/Types/ComparableOperationFilterInputType.cs

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

public sealed class LocalDateFilterInputType
    : ExtendedComparableOperationFilterInputType<LocalDateType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"LocalDate{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class LocalDateTimeFilterInputType
    : ExtendedComparableOperationFilterInputType<LocalDateTimeType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"LocalDateTime{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class LongFilterInputType
    : ExtendedComparableOperationFilterInputType<LongType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Long{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class LocalTimeFilterInputType
    : ExtendedComparableOperationFilterInputType<LocalTimeType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"LocalTime{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class FloatFilterInputType
    : ExtendedComparableOperationFilterInputType<FloatType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Float{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}

public sealed class DurationFilterInputType
    : ExtendedComparableOperationFilterInputType<DurationType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Duration{GraphQlConstants.FilterInputSuffix}");
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

public sealed class UriFilterInputType
    : ExtendedComparableOperationFilterInputType<MyUriType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name($"Url{GraphQlConstants.FilterInputSuffix}");
        base.Configure(descriptor);
    }
}