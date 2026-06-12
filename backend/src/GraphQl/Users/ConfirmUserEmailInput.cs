using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record ConfirmUserEmailInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string Email,
    string ConfirmationCode
);
