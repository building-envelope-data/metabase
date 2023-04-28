using System;

namespace Metabase.GraphQl.Users
{
    public sealed record DeleteUserInput(
          Guid UserId
        );
}