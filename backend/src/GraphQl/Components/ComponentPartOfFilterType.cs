using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentPartOfFilterType
    : ComponentAssemblies.ComponentAssemblyFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentPartOfFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.PartComponent).Ignore();
    }
}