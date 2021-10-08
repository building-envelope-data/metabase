using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public record LoginUserInput(
          [GraphQLType(typeof(EmailAddressType))] string Email,
          string Password,
          bool RememberMe
        );
}