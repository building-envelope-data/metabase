using System;

namespace Metabase.GraphQl.Users
{
    public record RemoveUserRoleInput(
          Guid UserId,
          Enumerations.UserRole Role
        );
}