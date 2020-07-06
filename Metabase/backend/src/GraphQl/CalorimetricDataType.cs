using HotChocolate.Types;

namespace Metabase.GraphQl
{
    public sealed class CalorimetricDataType
      : ObjectType<CalorimetricData>
    {
        protected override void Configure(IObjectTypeDescriptor<CalorimetricData> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}