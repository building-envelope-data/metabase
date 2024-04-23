namespace Metabase.GraphQl.Users
{
    public sealed record ChangeUserPasswordInput(
        string CurrentPassword,
        string NewPassword,
        string NewPasswordConfirmation
    );
}