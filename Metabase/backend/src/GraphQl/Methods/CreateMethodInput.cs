using System;

namespace Metabase.GraphQl.Methods
{
    public record CreateMethodInput(
          string Name,
          string Description,
          Uri? PublicationLocator,
          Uri? CodeLocator,
          Enumerations.MethodCategory[] Categories
        );
}