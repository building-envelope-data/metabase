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
            // TODO Use connections for concretization and generalization as is done above for manufacturers.
            descriptor
                .Field(t => t.ManufacturerEdges).Ignore();
            descriptor
                .Field(t => t.Generalizations)
                .Ignore();
            descriptor
                .Field(t => t.GeneralizationEdges)
                .Ignore();
            descriptor
                .Field(t => t.Concretizations)
                .Ignore();
            descriptor
                .Field(t => t.ConcretizationEdges)
                .Ignore();
        }
    }
}