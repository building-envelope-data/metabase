using System;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.Methods
{
    public record CreateMethodInput(
          string Name,
          string Description,
          OpenEndedDateTimeRangeInput? Validity,
          OpenEndedDateTimeRangeInput? Availability,
          CreateStandardInput? Standard,
          CreatePublicationInput? Publication,
          Uri? CalculationLocator,
          Enumerations.MethodCategory[] Categories
        );
}