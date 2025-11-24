using System;
using Metabase.Data;

namespace Metabase.GraphQl.ContactInformations;

public sealed record ContactInformationInput(
    string? PhoneNumber,
    string? PostalAddress,
    string? EmailAddress,
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
            PostalAddress,
            EmailAddress,
            WebsiteLocator
        );
    }
};