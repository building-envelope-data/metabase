using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class ComponentAssemblyFilterType
        : FilterInputType<Data.ComponentAssembly>
    {
        protected override void Configure(
            IFilterInputTypeDescriptor<Data.ComponentAssembly> descriptor
        )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.AssembledComponent);
            descriptor.Field(x => x.PartComponent);
            descriptor.Field(x => x.Index);
            descriptor.Field(x => x.PrimeSurface);
        }
    }
}