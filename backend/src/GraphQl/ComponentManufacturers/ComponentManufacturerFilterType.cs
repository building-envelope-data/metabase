using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.ComponentManufacturers;

public sealed class ComponentManufacturerFilterType
    : FilterInputType<ComponentManufacturer>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Component);
        descriptor.Field(x => x.Institution);
        descriptor.Field(x => x.Pending);
    }
}