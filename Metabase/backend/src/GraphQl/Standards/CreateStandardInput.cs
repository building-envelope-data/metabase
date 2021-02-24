using System;

namespace Metabase.GraphQl.Standards
{
    public record CreateStandardInput(
          string Title,
          string Abstract,
          string Section,
          int Year,
          Enumerations.Standardizer[] Standardizers,
          Uri Locator
        );
}