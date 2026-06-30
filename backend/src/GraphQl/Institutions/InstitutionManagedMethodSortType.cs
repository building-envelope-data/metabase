using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedMethodSortType
    : MethodSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<Method> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedMethodSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}