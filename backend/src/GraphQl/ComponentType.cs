using HotChocolate.Types;

namespace Icon.GraphQl
{
    public sealed class ComponentType
      : ObjectType<Component>
    {
        protected override void Configure(IObjectTypeDescriptor<Component> descriptor)
        {
            /* descriptor */
            /*   .Field(f => f.GetOpticalData(null!, null!)) */
            /*   .Type<NonNullType<ListType<NonNullType<AnyType>>>>(); */
        }
    }
}