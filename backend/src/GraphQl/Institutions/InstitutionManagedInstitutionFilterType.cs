using HotChocolate.Data.Filters;
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
        descriptor.Name(nameof(InstitutionManagedInstitutionFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(_ => _.Manager).Ignore();
    }
}
