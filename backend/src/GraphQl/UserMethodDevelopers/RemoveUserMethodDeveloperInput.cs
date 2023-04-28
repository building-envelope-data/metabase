using System;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public sealed record RemoveUserMethodDeveloperInput(
          Guid MethodId,
          Guid UserId
        );
}