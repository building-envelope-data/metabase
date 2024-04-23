using HotChocolate.Types;

namespace Metabase.GraphQl.Numerations
{
    public sealed class NumerationType
        : ObjectType<Data.Numeration>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Numeration> descriptor
        )
        {
        }
    }
}