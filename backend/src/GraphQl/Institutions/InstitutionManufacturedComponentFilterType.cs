using HotChocolate.Data.Filters;
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
        descriptor.Name(nameof(InstitutionManufacturedComponentFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Institution).Ignore();
    }
}