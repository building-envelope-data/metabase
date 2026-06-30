using HotChocolate.Data.Filters;
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
        descriptor.Name(nameof(InstitutionRepresentativeFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(_ => _.Institution).Ignore();
    }
}
