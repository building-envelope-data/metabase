using Models = Icon.Models;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;

namespace Icon.GraphQl
{
    public sealed class User
      : NodeBase
    {
        public static User FromModel(
            Models.User model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            // TODO Event source user, use its safe id, and use its timestamp
            return new User(
                id: ValueObjects.Id.From(model.Id).Value, // TODO Don't use `Value` as it may cause run-time errors.
                timestamp: ValueObjects.Timestamp.Now,
                requestTimestamp: requestTimestamp
                );
        }

        public User(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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