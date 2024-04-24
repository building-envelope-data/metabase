using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class EnableUserTwoFactorAuthenticatorPayload
    : UserPayload<EnableUserTwoFactorAuthenticatorError>
{
    public EnableUserTwoFactorAuthenticatorPayload(
        User user
    )
        : base(user)
    {
    }

    public EnableUserTwoFactorAuthenticatorPayload(
        User user,
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

    public IReadOnlyCollection<string>? TwoFactorRecoveryCodes { get; }
    public string? SharedKey { get; }
    public string? AuthenticatorUri { get; }
}