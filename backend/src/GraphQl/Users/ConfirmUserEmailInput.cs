namespace Metabase.GraphQl.Users;

public sealed record ConfirmUserEmailInput(
    string Email,
    string ConfirmationCode
);