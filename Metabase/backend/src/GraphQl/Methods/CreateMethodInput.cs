using System.Collections.Generic;
using NpgsqlTypes;
using DateTime = System.DateTime;

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