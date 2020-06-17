using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using CancellationToken = System.Threading.CancellationToken;
using DateTime = System.DateTime;
using IPageInfo = HotChocolate.Types.Relay.IPageInfo;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class RepresentedInstitutionEdge
      : Edge
    {
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public RepresentedInstitutionEdge(
            InstitutionRepresentative institutionRepresentative
            )
          : base(
              nodeId: institutionRepresentative.InstitutionId,
              timestamp: institutionRepresentative.Timestamp,
              requestTimestamp: institutionRepresentative.RequestTimestamp
              )
        {
            Role = institutionRepresentative.Role;
        }

        public Task<Institution> GetNode(
            [DataLoader] InstitutionDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(NodeId, RequestTimestamp)
                );
        }
    }
}