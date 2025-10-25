using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManufacturerFilterType
    : ComponentManufacturers.ComponentManufacturerFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentManufacturerFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Component).Ignore();
    }
}