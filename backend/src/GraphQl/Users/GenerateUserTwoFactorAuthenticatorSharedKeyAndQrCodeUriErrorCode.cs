using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users;

[SuppressMessage("Naming", "CA1707")]
public enum GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriErrorCode
{
    UNKNOWN,
    UNKNOWN_USER,
    RESETTING_AUTHENTICATOR_KEY_FAILED,
    GETTING_AUTHENTICATOR_KEY_FAILED,
    GETTING_EMAIL_FAILED
}