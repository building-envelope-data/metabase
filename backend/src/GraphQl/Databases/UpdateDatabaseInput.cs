using System;

namespace Metabase.GraphQl.Databases
{
    public record UpdateDatabaseInput(
          Guid DatabaseId,
          string Name,
          string Description,
          Uri Locator
        );
}