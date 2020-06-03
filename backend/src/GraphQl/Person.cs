using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using Models = Icon.Models;

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