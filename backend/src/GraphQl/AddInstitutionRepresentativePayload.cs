using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddInstitutionRepresentativePayload
      : Payload
    {
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id UserId { get; }
        /* public ValueObjects.InstitutionRepresentativeRole Role { get; } */
        public InstitutionRepresentativeEdge InstitutionRepresentativeEdge { get; }
        public RepresentedInstitutionEdge RepresentedInstitutionEdge { get; }

        public AddInstitutionRepresentativePayload(
            InstitutionRepresentative institutionRepresentative
            )
          : base(institutionRepresentative.RequestTimestamp)
        {
            InstitutionId = institutionRepresentative.InstitutionId;
            UserId = institutionRepresentative.UserId;
            InstitutionRepresentativeEdge = new InstitutionRepresentativeEdge(institutionRepresentative);
            RepresentedInstitutionEdge = new RepresentedInstitutionEdge(institutionRepresentative);
        }

        public Task<Institution> GetInstitution(
            [DataLoader] InstitutionForTimestampedIdDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(InstitutionId, RequestTimestamp)
                );
        }

        public Task<User> GetUser(
            /* TODO [DataLoader] UserForTimestampedIdDataLoader userLoader */
            )
        {
            return null!;
            /* return userLoader.LoadAsync( */
            /*     TimestampHelpers.TimestampId(UserId, RequestTimestamp) */
            /*     ); */
        }
    }
}