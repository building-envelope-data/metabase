namespace Metabase.GraphQl.Users;

public sealed class TwoFactorAuthentication
{
    public bool HasAuthenticator { get; }
    public bool IsEnabled { get; }
    public bool IsMachineRemembered { get; }
    public int RecoveryCodesLeftCount { get; }

    public TwoFactorAuthentication(
        bool hasAuthenticator,
        bool isEnabled,
        bool isMachineRemembered,
        int recoveryCodesLeftCount
    )
    {
        HasAuthenticator = hasAuthenticator;
        IsEnabled = isEnabled;
        IsMachineRemembered = isMachineRemembered;
        RecoveryCodesLeftCount = recoveryCodesLeftCount;
    }
}