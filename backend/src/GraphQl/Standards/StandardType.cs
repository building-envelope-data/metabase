using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.Standards;

public sealed class StandardType
    : ObjectType<Standard>
{
    protected override void Configure(
        IObjectTypeDescriptor<Standard> descriptor
    )
    {
    }
}