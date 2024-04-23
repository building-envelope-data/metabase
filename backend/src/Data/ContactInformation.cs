using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// TODO Use it somewhere or remove it! :)
namespace Metabase.Data;

[Owned]
public sealed class ContactInformation
{
    [Phone] public string? PhoneNumber { get; private set; }

    [MinLength(1)] public string? PostalAddress { get; private set; }

    [EmailAddress] public string? EmailAddress { get; private set; }

    [Url] public Uri? WebsiteLocator { get; private set; }

    public ContactInformation(
        string? phoneNumber,
        string? postalAddress,
        string? emailAddress,
        Uri? websiteLocator
    )
    {
        PhoneNumber = phoneNumber;
        PostalAddress = postalAddress;
        EmailAddress = emailAddress;
        WebsiteLocator = websiteLocator;
    }
}