using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class GenerateUserTwoFactorRecoveryCodesPayload
      : UserPayload<GenerateUserTwoFactorRecoveryCodesError>
    {
        public IReadOnlyCollection<string>? TwoFactorRecoveryCodes { get; }

        public GenerateUserTwoFactorRecoveryCodesPayload(
            Data.User user,
            IReadOnlyCollection<string> recoveryCodes
            )
          : base(user)
        {
            TwoFactorRecoveryCodes = recoveryCodes;
        }

        public GenerateUserTwoFactorRecoveryCodesPayload(
            GenerateUserTwoFactorRecoveryCodesError error
            )
          : base(error)
        {
        }

        public GenerateUserTwoFactorRecoveryCodesPayload(
            Data.User user,
            GenerateUserTwoFactorRecoveryCodesError error
            )
          : base(user, error)
        {
        }
    }
}