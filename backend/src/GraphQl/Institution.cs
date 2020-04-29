using Models = Icon.Models;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public sealed class Institution
      : NodeBase, Stakeholder
    {
        public static Institution FromModel(
            Models.Institution model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new Institution(
                id: model.Id,
                information: InstitutionInformation.FromModel(model.Information),
                publicKey: model.PublicKey?.Value,
                state: model.State,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public InstitutionInformation Information { get; }
        public string? PublicKey { get; }
        public ValueObjects.InstitutionState State { get; }

        public Institution(
            ValueObjects.Id id,
            InstitutionInformation information,
            string? publicKey,
            ValueObjects.InstitutionState state,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            Information = information;
            PublicKey = publicKey;
            State = state;
        }

        [UsePaging]
        public Task<IReadOnlyList<Person>> GetAffiliatedPersons(
            [Parent] Institution institution,
            [DataLoader] PersonsAffiliatedWithInstitutionIdentifiedByTimestampedIdDataLoader affiliatedPersonsLoader,
            IResolverContext context
            )
        {
            return affiliatedPersonsLoader.LoadAsync(
                TimestampHelpers.TimestampId(institution.Id, TimestampHelpers.Fetch(context))
                );
        }

        public sealed class PersonsAffiliatedWithInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Person, Models.Institution, Models.Person>
        {
            public PersonsAffiliatedWithInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Person.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Method>> GetDevelopedMethods(
            [Parent] Institution institution,
            [DataLoader] MethodsDevelopedByInstitutionIdentifiedByTimestampedIdDataLoader methodsLoader,
            IResolverContext context
            )
        {
            return methodsLoader.LoadAsync(
                TimestampHelpers.TimestampId(institution.Id, TimestampHelpers.Fetch(context))
                );
        }

        public sealed class MethodsDevelopedByInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Method, Models.Institution, Models.Method>
        {
            public MethodsDevelopedByInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Method.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Component>> GetManufacturedComponents(
            IResolverContext context
            )
        {
            /* ManufacturedComponentsConnection */
            /* return new ManufacturedComponentsConnection */
            return null!;
        }

        // TODO Return role information associated with representatives!
        public Task<IReadOnlyList<User>> GetRepresentatives(
            IEnumerable<ValueObjects.InstitutionRepresentativeRole>? roles,
            [Parent] Institution institution,
            [DataLoader] RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader representativesLoader,
            IResolverContext context
            )
        {
            return representativesLoader.LoadAsync(
                TimestampHelpers.TimestampId(institution.Id, TimestampHelpers.Fetch(context))
                );
        }

        public sealed class RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<User, Models.Institution, Models.User>
        {
            public RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(User.FromModel, queryBus)
            {
            }
        }
    }
}