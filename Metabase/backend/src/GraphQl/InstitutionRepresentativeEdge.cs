using System.Threading.Tasks;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
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