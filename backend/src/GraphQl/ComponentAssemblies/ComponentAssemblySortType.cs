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
        descriptor.Field(x => x.Index);
        descriptor.Field(x => x.PrimeSurface);
    }
}