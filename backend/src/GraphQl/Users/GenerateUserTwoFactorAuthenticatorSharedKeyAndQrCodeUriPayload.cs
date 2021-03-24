namespace Metabase.GraphQl.Users
{
    public sealed class GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload
      : UserPayload<GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriError>
    {
        public string? SharedKey { get; }
        public string? AuthenticatorUri { get; }

        public GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload(
            Data.User user,
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
    }
}