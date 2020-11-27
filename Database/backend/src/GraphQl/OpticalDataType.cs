using HotChocolate.Types;

namespace Database.GraphQl
{
    public sealed class OpticalDataType
      : ObjectType<OpticalData>
    {
        protected override void Configure(IObjectTypeDescriptor<OpticalData> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}