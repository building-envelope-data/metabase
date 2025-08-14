using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeFilterType
    : InstitutionRepresentatives.InstitutionRepresentativeFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionRepresentativeFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.Institution).Ignore();
    }
}