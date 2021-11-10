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
                .Field(t => t.Parts)
                .Name("assembledOf")
                .Type<NonNullType<ObjectType<ComponentAssembledOfConnection>>>()
                .Resolve(context =>
                    new ComponentAssembledOfConnection(
                        context.Parent<Data.Component>()
                        )
                    );
            descriptor
                .Field(t => t.PartEdges).Ignore();
            descriptor
                .Field(t => t.PartOf)
                .Type<NonNullType<ObjectType<ComponentPartOfConnection>>>()
                .Resolve(context =>
                    new ComponentPartOfConnection(
                        context.Parent<Data.Component>()
                        )
                    );
            descriptor
                .Field(t => t.PartOfEdges).Ignore();
            descriptor
                .Field(t => t.Generalizations)
                .Name("concretizationOf")
                .Type<NonNullType<ObjectType<ComponentConcretizationOfConnection>>>()
                .Resolve(context =>
                    new ComponentConcretizationOfConnection(
                        context.Parent<Data.Component>()
                        )
                    );
            descriptor
                .Field(t => t.GeneralizationEdges)
                .Ignore();
            descriptor
                .Field(t => t.Concretizations)
                .Name("generalizationOf")
                .Type<NonNullType<ObjectType<ComponentGeneralizationOfConnection>>>()
                .Resolve(context =>
                    new ComponentGeneralizationOfConnection(
                        context.Parent<Data.Component>()
                        )
                    );
            descriptor
                .Field(t => t.ConcretizationEdges)
                .Ignore();
            descriptor
                .Field(t => t.Variants)
                .Ignore();
            descriptor
                .Field(t => t.VariantEdges).Ignore();
            descriptor
                .Field(t => t.VariantOf)
                .Type<NonNullType<ObjectType<ComponentVariantOfConnection>>>()
                .Resolve(context =>
                    new ComponentVariantOfConnection(
                        context.Parent<Data.Component>()
                        )
                    );
            descriptor
                .Field(t => t.VariantOfEdges).Ignore();
        }
    }
}