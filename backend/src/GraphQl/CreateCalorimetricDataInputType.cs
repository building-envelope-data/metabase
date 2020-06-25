using HotChocolate.Types;

namespace Icon.GraphQl
{
    public sealed class CreateCalorimetricDataInputType
      : InputObjectType<CreateCalorimetricDataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateCalorimetricDataInput> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}