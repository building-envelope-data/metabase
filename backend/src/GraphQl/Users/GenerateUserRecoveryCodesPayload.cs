using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class GenerateUserTwoFactorRecoveryCodesPayload
    : UserPayload<GenerateUserTwoFactorRecoveryCodesError>
{
    public GenerateUserTwoFactorRecoveryCodesPayload(
        User user,
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
        User user,
        GenerateUserTwoFactorRecoveryCodesError error
    )
        : base(user, error)
    {
    }

    public IReadOnlyCollection<string>? TwoFactorRecoveryCodes { get; }
}