using Guid = System.Guid;

namespace Metabase.GraphQl.Users
{
    public record ConfirmUserEmailInput(
          string Email,
          string ConfirmationCode
        );
}