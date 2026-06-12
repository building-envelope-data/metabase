using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record ResetUserPasswordInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string Email,
    string Password,
    string PasswordConfirmation,
    string ResetCode
);
