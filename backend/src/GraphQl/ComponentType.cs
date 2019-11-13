using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ComponentType : ObjectType<Component>
    {
        protected override void Configure(IObjectTypeDescriptor<Component> descriptor)
        {
            descriptor.Name("Component");

            descriptor.Field(t => t.Id)
                .Type<NonNullType<UuidType>>();

            descriptor.Field(t => t.Timestamp)
                .Type<NonNullType<DateTimeType>>();

            descriptor.Field<ComponentResolvers>(t => t.GetVersions(default, default))
                .Type<NonNullType<ListType<NonNullType<ComponentVersionType>>>>();
        }
    }
}