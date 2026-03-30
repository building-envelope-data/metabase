using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.InstitutionRepresentatives;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionSortType
    : InstitutionRepresentativeSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserRepresentedInstitutionSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}