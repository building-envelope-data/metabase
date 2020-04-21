// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class PersonAggregate
      : EventSourcedAggregate, IConvertible<Models.Person>
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string EmailAddress { get; set; }
        public Uri WebsiteLocator { get; set; }

#nullable disable
        public PersonAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.PersonCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.PersonId;
            Name = data.Name;
            PhoneNumber = data.PhoneNumber;
            PostalAddress = data.PostalAddress;
            EmailAddress = data.EmailAddress;
            WebsiteLocator = data.WebsiteLocator;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                      base.Validate(),
                      ValidateNull(Name, nameof(Name)),
                      ValidateNull(PhoneNumber, nameof(PhoneNumber)),
                      ValidateNull(PostalAddress, nameof(PostalAddress)),
                      ValidateNull(EmailAddress, nameof(EmailAddress)),
                      ValidateNull(WebsiteLocator, nameof(WebsiteLocator))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(PhoneNumber, nameof(PhoneNumber)),
                  ValidateNonNull(PostalAddress, nameof(PostalAddress)),
                  ValidateNonNull(EmailAddress, nameof(EmailAddress)),
                  ValidateNonNull(WebsiteLocator, nameof(WebsiteLocator))
                  );
        }

        public Result<Models.Person, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Person, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var nameResult = ValueObjects.Name.From(Name);
            var phoneNumberResult = ValueObjects.PhoneNumber.From(PhoneNumber);
            var postalAddressResult = ValueObjects.PostalAddress.From(PostalAddress);
            var emailAddressResult = ValueObjects.EmailAddress.From(EmailAddress);
            var websiteLocatorResult = ValueObjects.AbsoluteUri.From(WebsiteLocator);
            var contactInformationResult =
              ValueObjects.ContactInformation.From(
                  phoneNumber: PhoneNumber,
                  postalAddress: PostalAddress,
                  emailAddress: EmailAddress,
                  websiteLocator: WebsiteLocator
                  );
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  nameResult,
                  contactInformationResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Person.From(
                    id: idResult.Value,
                    name: nameResult.Value,
                    contactInformation: contactInformationResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}