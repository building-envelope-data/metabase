using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class Person
      : Stakeholder
    {
        public ValueObjects.Name Name { get; }
        public ValueObjects.ContactInformation ContactInformation { get; }

        private Person(
            Id id,
            ValueObjects.Name name,
            ValueObjects.ContactInformation contactInformation,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            ContactInformation = contactInformation;
        }

        public static Result<Person, Errors> From(
            Id id,
            ValueObjects.Name name,
            ValueObjects.ContactInformation contactInformation,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<Person, Errors>(
                  new Person(
            id: id,
            name: name,
            contactInformation: contactInformation,
            timestamp: timestamp
            )
                  );
        }
    }
}