using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentVersionType : ObjectType<ComponentVersion>
    {
        protected override void Configure(IObjectTypeDescriptor<ComponentVersion> descriptor)
        {
            descriptor.Name("ComponentVersion");

            descriptor.Field(f => f.Id)
                .Type<NonNullType<UuidType>>();

            descriptor.Field(f => f.ComponentId)
                .Type<NonNullType<UuidType>>();

            descriptor.Field(f => f.Timestamp)
                .Type<NonNullType<DateTimeType>>();
        }
    }
}