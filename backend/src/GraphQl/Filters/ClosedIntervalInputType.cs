using HotChocolate.Types;

namespace Metabase.GraphQl.Filters;

internal sealed class ClosedIntervalInputType<TSchemaType, TRuntimeType>
    : InputObjectType<ClosedIntervalInput<TRuntimeType>>
    where TSchemaType : class, IInputType
    where TRuntimeType : notnull
{
    protected override void Configure(
        IInputObjectTypeDescriptor<ClosedIntervalInput<TRuntimeType>> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor
            .Field(f => f.LowerBound)
            .Type<TSchemaType>();
        descriptor
            .Field(f => f.UpperBound)
            .Type<TSchemaType>();
    }
}