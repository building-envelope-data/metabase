using HotChocolate.Types;

namespace Infrastructure.GraphQl
{
    public sealed class NodeType
      : UnionType<INode>
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor.Name(typeof(INode).Name.Substring(1));
        }
    }
}