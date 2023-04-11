using System;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public record RemoveUserMethodDeveloperInput(
          Guid MethodId,
          Guid UserId
        );
}