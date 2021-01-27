using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionRepresentativeEdge
        : Edge<Data.User>
    {
        public Enumerations.InstitutionRepresentativeRole Role { get; }

        public InstitutionRepresentativeEdge(
            Data.InstitutionRepresentative institutionRepresentative
        )
            : base(institutionRepresentative.UserId)
        {
            Role = institutionRepresentative.Role;
        }

        public Task<Data.User> GetNode(
            [DataLoader] UserByIdDataLoader userById
            )
        {
            return userById.LoadAsync(NodeId)!;
        }
    }
}