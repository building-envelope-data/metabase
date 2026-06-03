using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Components;

public class ComponentSortType
    : AuditableEntitySortType<Component>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Abbreviation);
        descriptor.Field(_ => _.Description);
        // TODO Allow sorting by Availability. How? See https://chillicream.com/docs/hotchocolate/fetching-data/sorting/#customization
    }
}
