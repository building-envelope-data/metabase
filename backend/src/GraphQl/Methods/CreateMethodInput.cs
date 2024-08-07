using System;
using Metabase.Enumerations;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.Methods;

public sealed record CreateMethodInput(
    string Name,
    string Description,
    OpenEndedDateTimeRangeInput? Validity,
    OpenEndedDateTimeRangeInput? Availability,
    CreateStandardInput? Standard,
    CreatePublicationInput? Publication,
    Uri? CalculationLocator,
    MethodCategory[] Categories,
    Guid ManagerId,
    Guid[] InstitutionDeveloperIds,
    Guid[] UserDeveloperIds
);