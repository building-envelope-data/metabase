using System;
using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record RequestUserPasswordResetInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string Email,
    Uri? ReturnTo
);
