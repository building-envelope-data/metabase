using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record ResendUserEmailConfirmationInput(
    [property: GraphQLType<NonNullType<EmailAddressType>>] string Email
);
