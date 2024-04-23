using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriError
        : GraphQl.UserErrorBase<GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriErrorCode>
    {
        public GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriError(
            GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
            : base(code, message, path)
        {
        }
    }
}