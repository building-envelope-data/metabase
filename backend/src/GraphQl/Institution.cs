using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Types.Relay;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using Models = Icon.Models;

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
            [DataLoader] PersonsAffiliatedWithInstitutionDataLoader affiliatedPersonsLoader
            )
        {
            return affiliatedPersonsLoader.LoadAsync(
                TimestampHelpers.TimestampId(institution.Id, institution.RequestTimestamp)
                );
        }

        public sealed class PersonsAffiliatedWithInstitutionDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Person, Models.Institution, Models.PersonAffiliation, Models.Person>
        {
            public PersonsAffiliatedWithInstitutionDataLoader(IQueryBus queryBus)
              : base(Person.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Method>> GetDevelopedMethods(
            [Parent] Institution institution,
            [DataLoader] MethodsDevelopedByInstitutionDataLoader methodsLoader
            )
        {
            return methodsLoader.LoadAsync(
                TimestampHelpers.TimestampId(institution.Id, institution.RequestTimestamp)
                );
        }

        public sealed class MethodsDevelopedByInstitutionDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Method, Models.Institution, Models.MethodDeveloper, Models.Method>
        {
            public MethodsDevelopedByInstitutionDataLoader(IQueryBus queryBus)
              : base(Method.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Database>> GetOperatedDatabases(
            [Parent] Institution institution,
            [DataLoader] DatabasesOperatedByInstitutionDataLoader databasesLoader
            )
        {
            return databasesLoader.LoadAsync(
                TimestampHelpers.TimestampId(institution.Id, institution.RequestTimestamp)
                );
        }

        public sealed class DatabasesOperatedByInstitutionDataLoader
            : OneToManyAssociatesOfModelDataLoader<Database, Models.Institution, Models.Database>
        {
            public DatabasesOperatedByInstitutionDataLoader(IQueryBus queryBus)
              : base(Database.FromModel, queryBus)
            {
            }
        }

        public ManufacturedComponentConnection GetManufacturedComponents(
            [Parent] Institution institution
            )
        {
            return new ManufacturedComponentConnection(institution);
        }

        public InstitutionRepresentativeConnection GetRepresentatives(
            IEnumerable<ValueObjects.InstitutionRepresentativeRole>? roles,
            [Parent] Institution institution
            )
        {
            // TODO Filter by roles
            return new InstitutionRepresentativeConnection(institution);
        }
    }
}