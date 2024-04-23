namespace Metabase.GraphQl.Users
{
    public sealed record SetUserPasswordInput(
        string Password,
        string PasswordConfirmation
    );
}