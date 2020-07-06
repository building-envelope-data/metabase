using HotChocolate.Types;

namespace Metabase.GraphQl
{
    public sealed class CreateHygrothermalDataInputType
      : InputObjectType<CreateHygrothermalDataInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateHygrothermalDataInput> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}