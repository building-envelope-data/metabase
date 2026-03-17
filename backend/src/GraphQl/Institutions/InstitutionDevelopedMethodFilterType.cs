using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.InstitutionMethodDevelopers;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodFilterType
    : InstitutionMethodDeveloperFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionDevelopedMethodFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Institution).Ignore();
    }
}