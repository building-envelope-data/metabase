using System;

namespace Metabase.GraphQl.Users;

public sealed record RemoveUserRoleInput(
    Guid UserId,
    Enumerations.UserRole Role
);