using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.InstitutionMethodDevelopers;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodSortType
    : InstitutionMethodDeveloperSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionDevelopedMethodSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}