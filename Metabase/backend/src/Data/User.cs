using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

// TODO Make `User`, `Role`, ... subtype `Entity` and use `xmin` to catch update conflicts. Add interface `IEntity`.
namespace Metabase.Data
{
    public sealed class User
        : IdentityUser<Guid>,
          Infrastructure.Data.IEntity
    {
        public ICollection<UserClaim> Claims { get; } = new List<UserClaim>();
        public ICollection<UserLogin> Logins { get; } = new List<UserLogin>();
        public ICollection<UserToken> Tokens { get; } = new List<UserToken>();
        public ICollection<UserRole> Roles { get; } = new List<UserRole>();

        public ICollection<InstitutionRepresentative> RepresentedInstitutionEdges { get; } = new List<InstitutionRepresentative>();

        public ICollection<Institution> RepresentedInstitutions { get; } = new List<Institution>();

        public uint xmin { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
    }
}