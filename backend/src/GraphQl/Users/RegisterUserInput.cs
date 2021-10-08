using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public record RegisterUserInput(
          string Name,
          [GraphQLType(typeof(EmailAddressType))] string Email,
          string Password,
          string PasswordConfirmation
        );
}