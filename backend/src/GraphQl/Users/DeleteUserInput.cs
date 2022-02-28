using System;

namespace Metabase.GraphQl.Users
{
    public record DeleteUserInput(
          Guid UserId
        );
}