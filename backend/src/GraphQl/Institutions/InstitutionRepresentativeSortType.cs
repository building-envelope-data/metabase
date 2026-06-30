using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeSortType
    : InstitutionRepresentatives.InstitutionRepresentativeSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionRepresentativeSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}