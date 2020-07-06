using HotChocolate.Types;

namespace Metabase.GraphQl
{
    public sealed class OpticalDataFromDatabaseType
      : ObjectType<OpticalDataFromDatabase>
    {
        protected override void Configure(IObjectTypeDescriptor<OpticalDataFromDatabase> descriptor)
        {
            // `AnyType` https://github.com/ChilliCream/hotchocolate/issues/1051#issuecomment-613576432
            descriptor
              .Field(t => t.Data)
              .Type<NonNullType<AnyType>>();
        }
    }
}