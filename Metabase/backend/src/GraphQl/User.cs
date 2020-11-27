using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class User
      : Node
    {
        public static User FromModel(
            Models.User model,
            Timestamp requestTimestamp
            )
        {
            // TODO Event source user, use its safe id, and use its timestamp
            return new User(
                id: Infrastructure.ValueObjects.Id.From(model.Id).Value, // TODO Don't use `Value` as it may cause run-time errors.
                timestamp: Timestamp.Now,
                requestTimestamp: requestTimestamp
                );
        }

        public User(
            Id id,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
        }

        // TODO Return role information associated with representedInstitutions!
        public Task<IReadOnlyList<Institution>> GetRepresentedInstitutions(
            IEnumerable<ValueObjects.InstitutionRepresentativeRole>? roles,
            [Parent] User user,
            [DataLoader] InstitutionsRepresentedByUserDataLoader representedInstitutionsLoader
            )
        {
            return representedInstitutionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(user.Id, user.RequestTimestamp)
                );
        }

        public sealed class InstitutionsRepresentedByUserDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Institution, Models.User, Models.InstitutionRepresentative, Models.Institution>
        {
            public InstitutionsRepresentedByUserDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}