namespace Metabase.GraphQl.Users
{
    public sealed record LoginUserWithRecoveryCodeInput(
        string RecoveryCode
    );
}