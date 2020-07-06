using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;
using Uri = System.Uri;

namespace Metabase.GraphQl
{
    public sealed class Database
      : NodeBase
    {
        public static Database FromModel(
            Models.Database model,
            Timestamp requestTimestamp
            )
        {
            return new Database(
                id: model.Id,
                name: model.Name,
                description: model.Description,
                locator: model.Locator,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public string Name { get; }
        public string Description { get; }
        public Uri Locator { get; }

        public Database(
            Id id,
            string name,
            string description,
            Uri locator,
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
            Description = description;
            Locator = locator;
        }

        public async Task<Id> GetOperatingInstitutionId(
            [Parent] Database database,
            [DataLoader] InstitutionOperatingDatabaseDataLoader institutionLoader
            )
        {
            return
              (
               await institutionLoader.LoadAsync(
                 TimestampHelpers.TimestampId(database.Id, database.RequestTimestamp)
                 )
              )
              .Id;
        }

        public Task<Institution> GetOperatingInstitution(
            [Parent] Database database,
            [DataLoader] InstitutionOperatingDatabaseDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(database.Id, database.RequestTimestamp)
                );
        }

        public sealed class InstitutionOperatingDatabaseDataLoader
            : BackwardOneToManyAssociateOfModelDataLoader<Institution, Models.Database, Models.InstitutionOperatedDatabase, Models.Institution>
        {
            public InstitutionOperatingDatabaseDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}