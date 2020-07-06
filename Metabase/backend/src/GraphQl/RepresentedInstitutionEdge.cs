using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
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