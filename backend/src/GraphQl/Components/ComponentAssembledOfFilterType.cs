using HotChocolate.Data.Filters;
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
        descriptor.Name(nameof(ComponentAssembledOfFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(_ => _.AssembledComponent).Ignore();
    }
}
