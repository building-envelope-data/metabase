using HotChocolate.Types;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentType
      : EntityType<Data.Component, ComponentByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Component> descriptor
            )
        {
            base.Configure(descriptor);
            descriptor
                .Field(t => t.Manufacturers)
                .Resolve(context =>
                    new ComponentManufacturerConnection(
                        context.Parent<Data.Component>()
                        )
                    );
            descriptor
                .Field(t => t.ManufacturerEdges).Ignore();
        }
    }
}