using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedInstitutionSortType
    : InstitutionSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<Institution> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedInstitutionSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}