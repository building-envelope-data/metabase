using Models = Icon.Models;
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
    public sealed class InstitutionRepresentativeEdge
      : Edge
    {
        public ValueObjects.InstitutionRepresentativeRole Role { get; }

        public InstitutionRepresentativeEdge(
            InstitutionRepresentative institutionRepresentative
            )
          : base(
              nodeId: institutionRepresentative.UserId,
              timestamp: institutionRepresentative.Timestamp,
              requestTimestamp: institutionRepresentative.RequestTimestamp
              )
        {
            Role = institutionRepresentative.Role;
        }

        public Task<User> GetNode(/* TODO [DataLoader] UserForTimestampedIdDataLoader userLoader */)
        {
            return null!;
            /* return userLoader.LoadAsync( */
            /*     TimestampHelpers.TimestampId(NodeId, RequestTimestamp) */
            /*     ); */
        }
    }
}