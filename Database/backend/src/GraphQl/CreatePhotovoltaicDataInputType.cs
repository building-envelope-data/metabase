using HotChocolate.Types;

namespace Database.GraphQl
{
    public sealed class CreatePhotovoltaicDataInputType
      : InputObjectType<CreatePhotovoltaicDataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreatePhotovoltaicDataInput> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}