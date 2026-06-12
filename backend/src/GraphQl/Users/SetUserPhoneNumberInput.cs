using HotChocolate;
using HotChocolate.Types;

namespace Metabase.GraphQl.Users;

public sealed record SetUserPhoneNumberInput(
    [property: GraphQLType<NonNullType<PhoneNumberType>>] string PhoneNumber
);