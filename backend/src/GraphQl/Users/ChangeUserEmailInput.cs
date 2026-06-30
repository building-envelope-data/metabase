using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record ChangeUserEmailInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string NewEmail
);