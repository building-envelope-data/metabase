using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentVersionInputType : InputObjectType<ComponentVersionInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ComponentVersionInput> descriptor)
        {
            descriptor.Name("ComponentVersionInput");

            descriptor.Field(f => f.ComponentId)
                .Type<NonNullType<UuidType>>();
        }
    }
}
