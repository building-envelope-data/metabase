using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload
    : UserPayload<GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriError>
{
    public GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload(
        User user,
        string sharedKey,
        string authenticatorUri
    )
        : base(user)
    {
        SharedKey = sharedKey;
        AuthenticatorUri = authenticatorUri;
    }

    public GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload(
        GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriError error
    )
        : base(error)
    {
    }

    public string? SharedKey { get; }
    public string? AuthenticatorUri { get; }
}