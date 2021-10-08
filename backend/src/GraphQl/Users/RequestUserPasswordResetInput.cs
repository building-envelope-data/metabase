using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public record RequestUserPasswordResetInput(
          [GraphQLType(typeof(EmailAddressType))] string Email
        );
}