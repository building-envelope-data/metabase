using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Methods;

public class MethodSortType
    : AuditableEntitySortType<Method>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Method> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.CalculationLocator);
    }
}