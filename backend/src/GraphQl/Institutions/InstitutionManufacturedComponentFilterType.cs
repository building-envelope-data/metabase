using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.ComponentManufacturers;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentFilterType
    : ComponentManufacturerFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManufacturedComponentFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.Institution).Ignore();
    }
}