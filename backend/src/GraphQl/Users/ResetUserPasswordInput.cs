using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public record ResetUserPasswordInput(
          [GraphQLType(typeof(EmailAddressType))] string Email,
          string Password,
          string PasswordConfirmation,
          string ResetCode
        );
}