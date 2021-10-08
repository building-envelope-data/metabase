using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public record SetUserPhoneNumberInput(
          [GraphQLType(typeof(PhoneNumberType))] string PhoneNumber
        );
}