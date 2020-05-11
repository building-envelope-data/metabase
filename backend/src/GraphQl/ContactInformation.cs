using System.Collections.Generic;
using Uri = System.Uri;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ContactInformation
    {
        public static ContactInformation FromModel(
            ValueObjects.ContactInformation model
            )
        {
            return new ContactInformation(
                phoneNumber: model.PhoneNumber,
                postalAddress: model.PostalAddress,
                emailAddress: model.EmailAddress,
                websiteLocator: model.WebsiteLocator
                );
        }

        public string PhoneNumber { get; }
        public string PostalAddress { get; }
        public string EmailAddress { get; }
        public Uri WebsiteLocator { get; }

        public ContactInformation(
            string phoneNumber,
            string postalAddress,
            string emailAddress,
            Uri websiteLocator
            )
        {
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
            EmailAddress = emailAddress;
            WebsiteLocator = websiteLocator;
        }
    }
}