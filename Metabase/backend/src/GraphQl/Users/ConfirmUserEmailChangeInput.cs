using Guid = System.Guid;

namespace Metabase.GraphQl.Users
{
  public record ConfirmUserEmailChangeInput(
        string CurrentEmail,
        string NewEmail,
        string ConfirmationCode
      );
}
