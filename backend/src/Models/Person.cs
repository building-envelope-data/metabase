using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class Person
      : Stakeholder
    {
        public ValueObjects.Name Name { get; }
        public ValueObjects.ContactInformation ContactInformation { get; }
        public IEnumerable<Institution> Affiliations { get; }

        private Person(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.ContactInformation contactInformation,
            IEnumerable<Institution> affiliations,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            ContactInformation = contactInformation;
            Affiliations = affiliations;
        }

        public static Result<Person, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Name name,
            ValueObjects.ContactInformation contactInformation,
            IEnumerable<Institution> affiliations,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<Person, Errors>(
                  new Person(
            id: id,
            name: name,
            contactInformation: contactInformation,
            affiliations: affiliations,
            timestamp: timestamp
            )
                  );
        }
    }
}