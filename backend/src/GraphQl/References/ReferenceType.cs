using HotChocolate.Types;

namespace Metabase.GraphQl.References;

public sealed class ReferenceType
    : InterfaceType<Data.IReference>
{
    protected override void Configure(IInterfaceTypeDescriptor<Data.IReference> descriptor)
    {
        descriptor.Name(nameof(Data.IReference)[1..]);
    }
}