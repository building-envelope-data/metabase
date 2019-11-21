using Validatable = Icon.Validatable;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ContactInformation
      : Validatable
    {
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
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              !string.IsNullOrWhiteSpace(PhoneNumber) &&
              !string.IsNullOrWhiteSpace(PostalAddress) &&
              !string.IsNullOrWhiteSpace(EmailAddress) &&
              WebsiteLocator.IsAbsoluteUri;
        }
    }
}