using HotChocolate.Types;

namespace Database.GraphQl
{
    public sealed class CreateOpticalDataInputType
      : InputObjectType<CreateOpticalDataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateOpticalDataInput> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}