using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentVersionType : NodeType<ComponentVersion>
    {
        protected override void Configure(IObjectTypeDescriptor<ComponentVersion> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ComponentVersion");

            descriptor.Field(t => t.Information)
                .Type<NonNullType<ComponentInformationType>>();

            descriptor.Field(f => f.ComponentId)
                .Type<NonNullType<UuidType>>();
        }
    }
}