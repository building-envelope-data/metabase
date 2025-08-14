using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.ComponentAssemblies;

public abstract class ComponentAssemblyFilterType
    : FilterInputType<ComponentAssembly>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.AssembledComponent);
        descriptor.Field(x => x.PartComponent);
        descriptor.Field(x => x.Index);
        descriptor.Field(x => x.PrimeSurface);
    }
}