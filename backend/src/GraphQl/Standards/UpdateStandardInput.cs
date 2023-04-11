using System;
using Metabase.GraphQl.Numerations;

namespace Metabase.GraphQl.Standards
{
    public record UpdateStandardInput(
          string? Title,
          string? Abstract,
          string? Section,
          int? Year,
          UpdateNumerationInput Numeration,
          Enumerations.Standardizer[] Standardizers,
          Uri? Locator
        );
}