using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Methods;

public sealed class MethodSortType
    : EntitySortType<Method>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Method> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.CalculationLocator);
        descriptor.Field(x => x.Manager);
    }
}