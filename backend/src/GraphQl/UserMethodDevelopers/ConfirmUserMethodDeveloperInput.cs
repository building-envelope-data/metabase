using System;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed record ConfirmUserMethodDeveloperInput(
    Guid MethodId,
    Guid UserId
);