using System.Collections.Generic;
using DateTime = System.DateTime;
using NpgsqlTypes;

namespace Metabase.GraphQl.Methods
{
    public record CreateMethodInput(
          string Name,
          string Description,
          string? PublicationLocator,
          string? CodeLocator,
          Enumerations.MethodCategory[] Categories
        );
}
