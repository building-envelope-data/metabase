using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.References;

public sealed class ReferenceType
    : UnionType<IReference>
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Name(nameof(IReference)[1..]);
    }
}