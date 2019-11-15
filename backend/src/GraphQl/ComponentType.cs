using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentType : NodeType<Component>
    {
        protected override void Configure(IObjectTypeDescriptor<Component> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("Component");

            descriptor.Field(t => t.Information)
                .Type<NonNullType<ComponentInformationType>>();

            descriptor.Field<ComponentResolvers>(t => t.GetVersions(default, default))
                .Type<NonNullType<ListType<NonNullType<ComponentVersionType>>>>();
        }
    }
}