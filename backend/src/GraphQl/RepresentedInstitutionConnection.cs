using Models = Icon.Models;
using System.Linq;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using IPageInfo = HotChocolate.Types.Relay.IPageInfo;

namespace Icon.GraphQl
{
    public sealed class RepresentedInstitutionConnection
      : Connection
    {
        public RepresentedInstitutionConnection(
            User user
            )
          : base(
              fromId: user.Id,
              pageInfo: null!,
              requestTimestamp: user.RequestTimestamp
              )
        {
        }

        public async Task<IReadOnlyList<RepresentedInstitutionEdge>> GetEdges(
            [DataLoader] InstitutionsRepresentedByUserIdentifiedByTimestampedIdAssociationDataLoader representedInstitutionsLoader
            )
        {
            return (await representedInstitutionsLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                )
              .Select(a => new RepresentedInstitutionEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class InstitutionsRepresentedByUserIdentifiedByTimestampedIdAssociationDataLoader
            : BackwardAssociationsOfModelIdentifiedByTimestampedIdDataLoader<InstitutionRepresentative, Models.User, Models.InstitutionRepresentative>
        {
            public InstitutionsRepresentedByUserIdentifiedByTimestampedIdAssociationDataLoader(IQueryBus queryBus)
              : base(InstitutionRepresentative.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Institution>> GetNodes(
            [DataLoader] InstitutionsRepresentedByUserIdentifiedByTimestampedIdDataLoader representedInstitutionsLoader
            )
        {
            return representedInstitutionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class InstitutionsRepresentedByUserIdentifiedByTimestampedIdDataLoader
            : BackwardAssociatesOfModelIdentifiedByTimestampedIdDataLoader<Institution, Models.User, Models.InstitutionRepresentative, Models.Institution>
        {
            public InstitutionsRepresentedByUserIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}