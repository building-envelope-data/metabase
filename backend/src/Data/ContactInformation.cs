using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Owned]
public sealed class ContactInformation(
    string? phoneNumber,
    string? postalAddress,
    string? emailAddress,
    Uri? websiteLocator
)
{
    [Phone] public string? PhoneNumber { get; private set; } = phoneNumber;

    [MinLength(1)] public string? PostalAddress { get; private set; } = postalAddress;

    [EmailAddress] public string? EmailAddress { get; private set; } = emailAddress;

    [Url] public Uri? WebsiteLocator { get; private set; } = websiteLocator;

    // To evade the error
    // ---
    // Entity type `ContactInformation` is an optional dependent using table
    // sharing and containing other dependents without any required non shared
    // property to identify whether the entity exists. If all nullable
    // properties contain a null value in database then an object instance
    // won't be created in the query causing nested dependent's values to be
    // lost. Add a required property to create instances with null values for
    // other properties or mark the incoming navigation as required to always
    // create an instance.
    // ---
    // I introduce this non-null property.
    public bool Exists { get; private set; } = true;
}