using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentAssemblies;

public abstract class ComponentAssemblyFilterType
    : AuditableAssociationFilterType<ComponentAssembly>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.AssembledComponent);
        descriptor.Field(x => x.PartComponent);
        descriptor.Field(x => x.Index);
        descriptor.Field(x => x.PrimeSurface);
    }
}