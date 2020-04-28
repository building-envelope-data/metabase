using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            // TODO Event source user and use its timestamp
            return new User(
                id: model.Id,
                timestamp: DateTime.MinValue,
                requestTimestamp: requestTimestamp
                );
        }

        public User(
            Guid id,
            DateTime timestamp,
            DateTime requestTimestamp
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
            [DataLoader] InstitutionsRepresentedByUserIdentifiedByTimestampedIdDataLoader representedInstitutionsLoader,
            IResolverContext context
            )
        {
            return representedInstitutionsLoader.LoadAsync(
                TimestampId(user.Id, GraphQl.Timestamp.Fetch(context)),
                default(CancellationToken)
                );
        }

        public sealed class InstitutionsRepresentedByUserIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Institution, Models.User, Models.Institution>
        {
            public InstitutionsRepresentedByUserIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}