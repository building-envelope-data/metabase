using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.ComponentVariants;

public abstract class ComponentVariantFilterType
    : FilterInputType<ComponentVariant>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentVariant> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.OfComponent);
        descriptor.Field(x => x.ToComponent);
    }
}