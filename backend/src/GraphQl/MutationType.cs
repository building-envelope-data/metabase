using HotChocolate.Types;

namespace Icon.GraphQl
{
    public class MutationType
        : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(
                #nullable disable
                f => f.CreateComponent(default, default)
                #nullable enable
                )
                .Type<NonNullType<ComponentType>>();
            /* .Argument("component", a => a.Type<NonNullType<ComponentInputType>>()); */

            descriptor.Field(
                #nullable disable
                f => f.CreateComponentVersion(default, default)
                #nullable enable
                )
                .Type<NonNullType<ComponentVersionType>>()
                .Argument("componentVersionInput", a => a.Type<NonNullType<ComponentVersionInputType>>());
        }
    }
}