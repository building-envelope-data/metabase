using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using Uri = System.Uri;

namespace Metabase.ValueObjects
{
    public sealed class ContactInformation
      : ValueObject
    {
        public PhoneNumber PhoneNumber { get; }
        public PostalAddress PostalAddress { get; }
        public EmailAddress EmailAddress { get; }
        public AbsoluteUri WebsiteLocator { get; }

        private ContactInformation(
            PhoneNumber phoneNumber,
            PostalAddress postalAddress,
            EmailAddress emailAddress,
            AbsoluteUri websiteLocator
            )
        {
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
            EmailAddress = emailAddress;
            WebsiteLocator = websiteLocator;
        }

        public static Result<ContactInformation, Errors> From(
            PhoneNumber phoneNumber,
            PostalAddress postalAddress,
            EmailAddress emailAddress,
            AbsoluteUri websiteLocator,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<ContactInformation, Errors>(
                new ContactInformation(
                  phoneNumber: phoneNumber,
                  postalAddress: postalAddress,
                  emailAddress: emailAddress,
                  websiteLocator: websiteLocator
                  )
                );
        }

        public static Result<ContactInformation, Errors> From(
            string phoneNumber,
            string postalAddress,
            string emailAddress,
            Uri websiteLocator,
            IReadOnlyList<object>? path = null
            )
        {
            var phoneNumberResult = PhoneNumber.From(phoneNumber);
            var postalAddressResult = PostalAddress.From(postalAddress);
            var emailAddressResult = EmailAddress.From(emailAddress);
            var websiteLocatorResult = AbsoluteUri.From(websiteLocator);

            return Errors.Combine(
                phoneNumberResult,
                postalAddressResult,
                emailAddressResult,
                websiteLocatorResult
                )
              .Bind(_ =>
                  From(
                    phoneNumber: phoneNumberResult.Value,
                    postalAddress: postalAddressResult.Value,
                    emailAddress: emailAddressResult.Value,
                    websiteLocator: websiteLocatorResult.Value,
                    path: path
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PhoneNumber;
            yield return PostalAddress;
            yield return EmailAddress;
            yield return WebsiteLocator;
        }
    }
}