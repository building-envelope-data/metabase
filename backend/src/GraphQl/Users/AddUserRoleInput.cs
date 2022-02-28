using System;

namespace Metabase.GraphQl.Users
{
    public record AddUserRoleInput(
          Guid UserId,
          Enumerations.UserRole Role
        );
}