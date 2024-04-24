using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.Users;

public sealed record RemoveUserRoleInput(
    Guid UserId,
    UserRole Role
);