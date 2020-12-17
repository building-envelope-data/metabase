using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;
using System.Collections.Generic;

namespace Metabase.Data
{
    public sealed class Role : IdentityRole<Guid>
    {
      public ICollection<UserRole> UserRoles { get; private set; }

      public Role()
        : this(new List<UserRole>())
        {
        }

      public Role(
          ICollection<UserRole> userRoles
          )
      {
          UserRoles = userRoles;
      }
    }
}
