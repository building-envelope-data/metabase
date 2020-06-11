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

        public Task<User> GetNode(/* TODO [DataLoader] UserDataLoader userLoader */)
        {
            return null!;
            /* return userLoader.LoadAsync( */
            /*     TimestampHelpers.TimestampId(NodeId, RequestTimestamp) */
            /*     ); */
        }
    }
}