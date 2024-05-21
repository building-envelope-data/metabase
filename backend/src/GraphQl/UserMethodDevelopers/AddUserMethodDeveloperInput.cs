using System;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed record AddUserMethodDeveloperInput(
    Guid MethodId,
    Guid UserId
);