using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ContactInformation
      : Model
    {
        public string PhoneNumber { get; }
        public string PostalAddress { get; }
        public string EmailAddress { get; }
        public Uri WebsiteLocator { get; }

        public ContactInformation(
            Guid id,
            string phoneNumber,
            string postalAddress,
            string emailAddress,
            Uri websiteLocator,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
            EmailAddress = emailAddress;
            WebsiteLocator = websiteLocator;
        }
    }
}