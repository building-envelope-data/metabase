using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentAssembledOfFilterType
    : ComponentAssemblies.ComponentAssemblyFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentAssembledOfFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.AssembledComponent).Ignore();
    }
}