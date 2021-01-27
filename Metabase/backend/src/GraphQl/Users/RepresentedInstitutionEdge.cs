using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Users
{
    public sealed class RepresentedInstitutionEdge
        : Edge<Data.Institution>
    {
        public Enumerations.InstitutionRepresentativeRole Role { get; }

        public RepresentedInstitutionEdge(
            Data.InstitutionRepresentative institutionRepresentative
        )
            : base(institutionRepresentative.UserId)
        {
            Role = institutionRepresentative.Role;
        }

        public Task<Data.Institution> GetNode(
            [DataLoader] InstitutionByIdDataLoader institutionById
            )
        {
            return institutionById.LoadAsync(NodeId)!;
        }
    }
}