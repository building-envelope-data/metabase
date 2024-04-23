namespace Metabase.GraphQl.Users
{
    public sealed record LoginUserInput(
        string Email,
        string Password
    );
}