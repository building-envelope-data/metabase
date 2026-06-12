using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record LoginUserInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string Email,
    string Password
);
