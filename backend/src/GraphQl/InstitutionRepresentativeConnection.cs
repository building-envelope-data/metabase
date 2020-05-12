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
    public sealed class InstitutionRepresentativeConnection
      : Connection
    {
        public InstitutionRepresentativeConnection(
            Institution institution
            )
          : base(
              fromId: institution.Id,
              pageInfo: null!,
              requestTimestamp: institution.RequestTimestamp
              )
        {
        }

        public async Task<IReadOnlyList<InstitutionRepresentativeEdge>> GetEdges(
            [DataLoader] RepresentativesOfInstitutionAssociationDataLoader representativesLoader
            )
        {
            return (await representativesLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                .ConfigureAwait(false)
                )
              .Select(a => new InstitutionRepresentativeEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class RepresentativesOfInstitutionAssociationDataLoader
            : ForwardAssociationsOfModelDataLoader<InstitutionRepresentative, Models.Institution, Models.InstitutionRepresentative>
        {
            public RepresentativesOfInstitutionAssociationDataLoader(IQueryBus queryBus)
              : base(InstitutionRepresentative.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<User>> GetNodes(
            [DataLoader] RepresentativesOfInstitutionDataLoader representativesLoader
            )
        {
            return representativesLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class RepresentativesOfInstitutionDataLoader
            : ForwardAssociatesOfModelDataLoader<User, Models.Institution, Models.InstitutionRepresentative, Models.User>
        {
            public RepresentativesOfInstitutionDataLoader(IQueryBus queryBus)
              : base(User.FromModel, queryBus)
            {
            }
        }
    }
}