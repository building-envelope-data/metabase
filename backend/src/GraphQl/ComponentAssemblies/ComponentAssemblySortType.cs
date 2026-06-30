using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentAssemblies;

public abstract class ComponentAssemblySortType
    : AuditableAssociationSortType<ComponentAssembly>
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(_ => _.Index);
        descriptor.Field(_ => _.PrimeSurface);
    }
}
