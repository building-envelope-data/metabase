using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class Person
      : Stakeholder
    {
        public ValueObjects.Name Name { get; }
        public ValueObjects.ContactInformation ContactInformation { get; }

        private Person(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.ContactInformation contactInformation,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            ContactInformation = contactInformation;
        }

        public static Result<Person, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.ContactInformation contactInformation,
            ValueObjects.Timestamp timestamp
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