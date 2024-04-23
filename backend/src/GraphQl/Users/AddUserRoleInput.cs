using System;

namespace Metabase.GraphQl.Users
{
    public sealed record AddUserRoleInput(
        Guid UserId,
        Enumerations.UserRole Role
    );
}