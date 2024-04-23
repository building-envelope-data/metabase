using Guid = System.Guid;

namespace Metabase.GraphQl.Users
{
    public sealed record ConfirmUserEmailChangeInput(
        string CurrentEmail,
        string NewEmail,
        string ConfirmationCode
    );
}