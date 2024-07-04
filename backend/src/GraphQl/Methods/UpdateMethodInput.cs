using System;
using Metabase.Enumerations;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.Methods;

public sealed record UpdateMethodInput(
    Guid MethodId,
    string Name,
    string Description,
    OpenEndedDateTimeRangeInput? Validity,
    OpenEndedDateTimeRangeInput? Availability,
    UpdateStandardInput? Standard,
    UpdatePublicationInput? Publication,
    Uri? CalculationLocator,
    MethodCategory[] Categories
);