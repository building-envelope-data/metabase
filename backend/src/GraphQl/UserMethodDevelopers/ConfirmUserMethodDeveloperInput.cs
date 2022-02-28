using System;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public record ConfirmUserMethodDeveloperInput(
          Guid MethodId,
          Guid UserId
        );
}