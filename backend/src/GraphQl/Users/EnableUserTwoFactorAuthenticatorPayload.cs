using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class EnableUserTwoFactorAuthenticatorPayload
    : UserPayload<EnableUserTwoFactorAuthenticatorError>
{
    public IReadOnlyCollection<string>? TwoFactorRecoveryCodes { get; }
    public string? SharedKey { get; }
    public string? AuthenticatorUri { get; }

    public EnableUserTwoFactorAuthenticatorPayload(
        Data.User user
    )
        : base(user)
    {
    }

    public EnableUserTwoFactorAuthenticatorPayload(
        Data.User user,
        IReadOnlyCollection<string> recoveryCodes
    )
        : base(user)
    {
        TwoFactorRecoveryCodes = recoveryCodes;
    }

    public EnableUserTwoFactorAuthenticatorPayload(
        EnableUserTwoFactorAuthenticatorError error,
        string sharedKey,
        string authenticatorUri
    )
        : base(error)
    {
        SharedKey = sharedKey;
        AuthenticatorUri = authenticatorUri;
    }

    public EnableUserTwoFactorAuthenticatorPayload(
        EnableUserTwoFactorAuthenticatorError error
    )
        : base(error)
    {
    }
}