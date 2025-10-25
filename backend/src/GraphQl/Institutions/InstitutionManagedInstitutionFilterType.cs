using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedInstitutionFilterType
    : InstitutionFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Institution> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedInstitutionFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Manager).Ignore();
    }
}