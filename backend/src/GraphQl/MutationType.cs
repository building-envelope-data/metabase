using HotChocolate.Types;

namespace Icon.GraphQl
{
    public class MutationType
        : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor
              .Field(f => f.CreateComponent(default!, default!))
                .Type<NonNullType<ComponentType>>()
              .Argument("input", a => a.Type<NonNullType<ComponentInputType>>());

            descriptor
              .Field(f => f.CreateComponentVersion(default!, default!))
                .Type<NonNullType<ComponentVersionType>>()
                .Argument("input", a => a.Type<NonNullType<ComponentVersionInputType>>());
        }
    }
}