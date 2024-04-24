namespace Metabase.GraphQl.Users;

public sealed record EnableUserTwoFactorAuthenticatorInput(
    string VerificationCode
);