using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;
using System.Collections.Generic;

// TODO Make `User`, `Role`, ... subtype `Entity` and use `xmin` to catch update conflicts. Add interface `IEntity`.
namespace Metabase.Data
{
    public sealed class User : IdentityUser<Guid>
    {
        public ICollection<UserClaim> Claims { get; private set; }
        public ICollection<UserLogin> Logins { get; private set; }
        public ICollection<UserToken> Tokens { get; private set; }
        public ICollection<UserRole> UserRoles { get; private set; }

        public ICollection<InstitutionRepresentative> RepresentedInstitutionEdges { get; } = new List<InstitutionRepresentative>();

        public ICollection<Institution> RepresentedInstitutions { get; } = new List<Institution>();

        public User()
          : this(
          new List<UserClaim>(),
          new List<UserLogin>(),
          new List<UserToken>(),
          new List<UserRole>()
              )
        {
        }

        public User(
            ICollection<UserClaim> claims,
            ICollection<UserLogin> logins,
            ICollection<UserToken> tokens,
            ICollection<UserRole> userRoles
            )
        {
            Claims = claims;
            Logins = logins;
            Tokens = tokens;
            UserRoles = userRoles;
        }
    }
}
