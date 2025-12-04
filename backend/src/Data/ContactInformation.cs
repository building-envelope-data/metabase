using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Owned]
public sealed class ContactInformation(
    string? phoneNumber,
    bool isPhoneNumberConfirmed,
    string? postalAddress,
    string? emailAddress,
    bool isEmailAddressConfirmed,
    Uri? websiteLocator
)
{
    [Phone] public string? PhoneNumber { get; private set; } = phoneNumber;

    public bool IsPhoneNumberConfirmed { get; private set; } = isPhoneNumberConfirmed;

    [MinLength(1)] public string? PostalAddress { get; private set; } = postalAddress;

    [EmailAddress] public string? EmailAddress { get; private set; } = emailAddress;

    public bool IsEmailAddressConfirmed { get; private set; } = isEmailAddressConfirmed;

    [Url] public Uri? WebsiteLocator { get; private set; } = websiteLocator;
}