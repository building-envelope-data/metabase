using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class Person
      : StakeholderBase
    {
        public static Person FromModel(
            Models.Person model,
            Timestamp requestTimestamp
            )
        {
            return new Person(
                id: model.Id,
                name: model.Name,
                contactInformation: ContactInformation.FromModel(model.ContactInformation),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public string Name { get; }
        public ContactInformation ContactInformation { get; }

        public Person(
            Id id,
            string name,
            ContactInformation contactInformation,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            Name = name;
            ContactInformation = contactInformation;
        }

        public Task<IReadOnlyList<Institution>> GetAffiliatedInstitutions(
            [Parent] Person person,
            [DataLoader] InstitutionsAffiliatedWithPersonDataLoader institutionsLoader
            )
        {
            return institutionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(person.Id, person.RequestTimestamp)
                );
        }

        public sealed class InstitutionsAffiliatedWithPersonDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Institution, Models.Person, Models.PersonAffiliation, Models.Institution>
        {
            public InstitutionsAffiliatedWithPersonDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Method>> GetDevelopedMethods(
            [Parent] Person person,
            [DataLoader] MethodsDevelopedByPersonDataLoader methodsLoader
            )
        {
            return methodsLoader.LoadAsync(
                TimestampHelpers.TimestampId(person.Id, person.RequestTimestamp)
                );
        }

        public sealed class MethodsDevelopedByPersonDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Method, Models.Person, Models.MethodDeveloper, Models.Method>
        {
            public MethodsDevelopedByPersonDataLoader(IQueryBus queryBus)
              : base(Method.FromModel, queryBus)
            {
            }
        }
    }
}