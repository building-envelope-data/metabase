using Uri = System.Uri;
using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class CreatePersonInput
    {
        public string Name { get; }
        public string PhoneNumber { get; }
        public string PostalAddress { get; }
        public string EmailAddress { get; }
        public Uri WebsiteLocator { get; }

        public CreatePersonInput(
            string name,
            string phoneNumber,
            string postalAddress,
            string emailAddress,
            Uri websiteLocator
            )
        {
            Name = name;
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
            EmailAddress = emailAddress;
            WebsiteLocator = websiteLocator;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.CreatePersonInput, Errors> Validate(
            CreatePersonInput self,
            IReadOnlyList<object> path
            )
        {
            var nameResult = ValueObjects.Name.From(
                self.Name,
                path.Append("name").ToList().AsReadOnly()
                );
            var phoneNumberResult = ValueObjects.PhoneNumber.From(
                self.PhoneNumber,
                path.Append("phoneNumber").ToList().AsReadOnly()
                );
            var postalAddressResult = ValueObjects.PostalAddress.From(
                self.PostalAddress,
                path.Append("postalAddress").ToList().AsReadOnly()
                );
            var emailAddressResult = ValueObjects.EmailAddress.From(
                self.EmailAddress,
                path.Append("emailAddress").ToList().AsReadOnly()
                );
            var websiteLocatorResult = ValueObjects.AbsoluteUri.From(
                self.WebsiteLocator,
                path.Append("websiteLocator").ToList().AsReadOnly()
                );

            return
              Errors.Combine(
                  nameResult,
                  phoneNumberResult,
                  postalAddressResult,
                  emailAddressResult,
                  websiteLocatorResult
                  )
              .Bind(_ =>
                  ValueObjects.CreatePersonInput.From(
                    name: nameResult.Value,
                    phoneNumber: phoneNumberResult.Value,
                    postalAddress: postalAddressResult.Value,
                    emailAddress: emailAddressResult.Value,
                    websiteLocator: websiteLocatorResult.Value
                    )
                  );
        }
    }
}