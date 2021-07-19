using HotChocolate.Types;
using Metabase.Extensions;

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
                .Argument(nameof(Data.ComponentManufacturer.Pending).FirstCharToLower(), _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
                .Type<NonNullType<ObjectType<ComponentManufacturerConnection>>>()
                .Resolve(context =>
                    new ComponentManufacturerConnection(
                        context.Parent<Data.Component>(),
                        context.ArgumentValue<bool>(nameof(Data.ComponentManufacturer.Pending).FirstCharToLower())
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