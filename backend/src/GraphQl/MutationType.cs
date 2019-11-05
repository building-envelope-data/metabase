using HotChocolate.Types;

namespace Icon.GraphQl
{
    public class MutationType
        : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(f => f.CreateComponent())
                .Type<NonNullType<ComponentType>>();
            /* .Argument("component", a => a.Type<NonNullType<ComponentInputType>>()); */

            descriptor.Field(f => f.CreateComponentVersion(default))
                .Type<NonNullType<ComponentVersionType>>()
                .Argument("componentVersionInput", a => a.Type<NonNullType<ComponentVersionInputType>>());
        }
    }
}