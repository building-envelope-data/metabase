using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Events;
using Icon.Infrastructure;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Commands = Icon.Commands;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;
using Uri = System.Uri;

namespace Icon.Events
{
    public sealed class PersonCreated
      : CreatedEvent
    {
        public static PersonCreated From(
            Guid personId,
            Commands.Create<ValueObjects.CreatePersonInput> command
            )
        {
            return new PersonCreated(
                personId: personId,
                name: command.Input.Name,
                phoneNumber: command.Input.PhoneNumber,
                postalAddress: command.Input.PostalAddress,
                emailAddress: command.Input.EmailAddress,
                websiteLocator: command.Input.WebsiteLocator,
                creatorId: command.CreatorId
                );
        }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string EmailAddress { get; set; }
        public Uri WebsiteLocator { get; set; }

#nullable disable
        public PersonCreated() { }
#nullable enable

        public PersonCreated(
            Guid personId,
            string name,
            string phoneNumber,
            string postalAddress,
            string emailAddress,
            Uri websiteLocator,
            Guid creatorId
            )
          : base(
              aggregateId: personId,
              creatorId: creatorId
              )
        {
            Name = name;
            PhoneNumber = phoneNumber;
            PostalAddress = postalAddress;
            EmailAddress = emailAddress;
            WebsiteLocator = websiteLocator;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(PhoneNumber, nameof(PhoneNumber)),
                  ValidateNonNull(PostalAddress, nameof(PostalAddress)),
                  ValidateNonNull(EmailAddress, nameof(EmailAddress)),
                  ValidateNonNull(WebsiteLocator, nameof(WebsiteLocator))
                  );
        }
    }
}