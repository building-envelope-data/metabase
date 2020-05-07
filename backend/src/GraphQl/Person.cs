using Models = Icon.Models;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.GraphQl
{
    public sealed class Person
      : NodeBase, Stakeholder
    {
        public static Person FromModel(
            Models.Person model,
            ValueObjects.Timestamp requestTimestamp
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
            ValueObjects.Id id,
            string name,
            ContactInformation contactInformation,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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
            [DataLoader] InstitutionsAffiliatedWithPersonIdentifiedByTimestampedIdDataLoader institutionsLoader
            )
        {
            return institutionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(person.Id, person.RequestTimestamp)
                );
        }

        public sealed class InstitutionsAffiliatedWithPersonIdentifiedByTimestampedIdDataLoader
            : ForwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Institution, Models.Person, Models.PersonAffiliation, Models.Institution>
        {
            public InstitutionsAffiliatedWithPersonIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Method>> GetDevelopedMethods(
            [Parent] Person person,
            [DataLoader] MethodsDevelopedByPersonIdentifiedByTimestampedIdDataLoader methodsLoader
            )
        {
            return methodsLoader.LoadAsync(
                TimestampHelpers.TimestampId(person.Id, person.RequestTimestamp)
                );
        }

        public sealed class MethodsDevelopedByPersonIdentifiedByTimestampedIdDataLoader
            : BackwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Method, Models.Person, Models.MethodDeveloper, Models.Method>
        {
            public MethodsDevelopedByPersonIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Method.FromModel, queryBus)
            {
            }
        }
    }
}