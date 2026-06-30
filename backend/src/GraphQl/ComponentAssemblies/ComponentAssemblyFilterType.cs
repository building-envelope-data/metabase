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
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.AssembledComponent);
        descriptor.Field(_ => _.PartComponent);
        descriptor.Field(_ => _.Index);
        descriptor.Field(_ => _.PrimeSurface);
    }
}
