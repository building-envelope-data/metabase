using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Components;

public class ComponentFilterType
    : EntityFilterType<Component>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Abbreviation);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.Categories);
        descriptor.Field(x => x.Extras);
        descriptor.Field(x => x.PartOf);
        descriptor.Field(x => x.Parts);
        descriptor.Field(x => x.PartOfEdges);
        descriptor.Field(x => x.PartEdges);
        descriptor.Field(x => x.Concretizations);
        descriptor.Field(x => x.Generalizations);
        descriptor.Field(x => x.Variants);
        descriptor.Field(x => x.Manager);
        descriptor.Field(x => x.Manufacturers);
        descriptor.Field(x => x.ManufacturerEdges);
        // TODO Allow filtering by Availability. How? See https://chillicream.com/docs/hotchocolate/fetching-data/filtering/#customization
    }
}