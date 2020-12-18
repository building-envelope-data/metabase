using Guid = System.Guid;

namespace Metabase.GraphQl.Users
{
  public record ConfirmUserEmailChangeInput(
        string OldEmail,
        string NewEmail,
        string ConfirmationCode
      );
}
