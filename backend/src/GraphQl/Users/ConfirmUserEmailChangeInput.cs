using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record ConfirmUserEmailChangeInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string CurrentEmail,
    [property: GraphQLType<NonNullType<EmailAddressType>>] string NewEmail,
    string ConfirmationCode
);
