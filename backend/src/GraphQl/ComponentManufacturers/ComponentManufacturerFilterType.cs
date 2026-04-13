using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.ComponentManufacturers;

public abstract class ComponentManufacturerFilterType
    : FilterInputType<ComponentManufacturer>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Component);
        descriptor.Field(x => x.Institution);
    }
}