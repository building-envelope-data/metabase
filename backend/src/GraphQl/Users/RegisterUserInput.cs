using System;

namespace Metabase.GraphQl.Users;

public sealed record RegisterUserInput(
    string Name,
    string Email,
    string Password,
    string PasswordConfirmation,
    Uri? ReturnTo
);