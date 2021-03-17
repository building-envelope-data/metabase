using System;

namespace Metabase.GraphQl.Databases
{
    public record CreateDatabaseInput(
          string Name,
          string Description,
          Uri Locator,
          Guid OperatorId
        );
}