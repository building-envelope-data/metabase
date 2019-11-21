using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public abstract class NodeType<T>
      : ObjectType<T>
      where T : INode
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            descriptor.Field(t => t.Id)
                .Type<NonNullType<UuidType>>();

            descriptor.Field(t => t.Timestamp)
                .Type<NonNullType<DateTimeType>>();

            descriptor.Field(f => f.RequestTimestamp)
                .Type<NonNullType<DateTimeType>>();
        }
    }
}