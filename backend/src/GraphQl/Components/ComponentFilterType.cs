using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Components;

public class ComponentFilterType
    : AuditableEntityFilterType<Component>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Abbreviation);
        descriptor.Field(_ => _.Description);
        descriptor.Field(_ => _.Categories);
        descriptor.Field(_ => _.Extras);
        descriptor.Field(_ => _.PartOf);
        descriptor.Field(_ => _.Parts);
        descriptor.Field(_ => _.PartOfEdges);
        descriptor.Field(_ => _.PartEdges);
        descriptor.Field(_ => _.Concretizations);
        descriptor.Field(_ => _.Generalizations);
        descriptor.Field(_ => _.Variants);
        descriptor.Field(_ => _.Manager);
        descriptor.Field(_ => _.Manufacturers);
        descriptor.Field(_ => _.ManufacturerEdges);
        // TODO Allow filtering by Availability. How? See https://chillicream.com/docs/hotchocolate/fetching-data/filtering/#customization
    }
}
