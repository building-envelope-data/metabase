using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Components;

public sealed class ComponentSortType
    : EntitySortType<Component>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Abbreviation);
        descriptor.Field(x => x.Description);
        // TODO Allow sorting by Availability. How? See https://chillicream.com/docs/hotchocolate/fetching-data/sorting/#customization
    }
}