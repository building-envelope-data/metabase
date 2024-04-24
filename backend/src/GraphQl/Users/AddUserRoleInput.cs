using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.Users;

public sealed record AddUserRoleInput(
    Guid UserId,
    UserRole Role
);