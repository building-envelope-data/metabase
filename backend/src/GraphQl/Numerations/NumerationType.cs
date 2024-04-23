using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.Numerations;

public sealed class NumerationType
    : ObjectType<Numeration>
{
    protected override void Configure(
        IObjectTypeDescriptor<Numeration> descriptor
    )
    {
    }
}