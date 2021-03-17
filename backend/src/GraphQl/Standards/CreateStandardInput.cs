using System;
using Metabase.GraphQl.Numerations;

namespace Metabase.GraphQl.Standards
{
    public record CreateStandardInput(
          string? Title,
          string? Abstract,
          string? Section,
          int? Year,
          CreateNumerationInput Numeration,
          Enumerations.Standardizer[] Standardizers,
          Uri? Locator
        );
}