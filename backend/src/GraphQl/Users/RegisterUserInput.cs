using System;
using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record RegisterUserInput(
    string Name,
    [property: GraphQLType<NonNullType<EmailAddressType>>] string Email,
    string Password,
    string PasswordConfirmation,
    Uri? ReturnTo
);
