using System;

namespace Metabase.GraphQl.Users;

public sealed record RequestUserPasswordResetInput(
    string Email,
    Uri? ReturnTo
);