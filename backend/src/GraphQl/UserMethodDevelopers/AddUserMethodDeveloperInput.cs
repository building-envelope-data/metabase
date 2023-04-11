using System;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public record AddUserMethodDeveloperInput(
          Guid MethodId,
          Guid UserId
        );
}