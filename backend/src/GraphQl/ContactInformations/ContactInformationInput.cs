using System;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data;

namespace Metabase.GraphQl.ContactInformations;

public sealed record ContactInformationInput(
    [property: GraphQLType<PhoneNumberType>] string? PhoneNumber,
    string? PostalAddress,
    [property: GraphQLType<EmailAddressType>] string? EmailAddress,
    Uri? WebsiteLocator
)
{
    public ContactInformation? ToDomainModel()
    {
        if (PhoneNumber is null
            && PostalAddress is null
            && EmailAddress is null
            && WebsiteLocator is null
        )
        {
            return null;
        }
        return new(
            PhoneNumber,
            false,
            PostalAddress,
            EmailAddress,
            false,
            WebsiteLocator
        );
    }
};