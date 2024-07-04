using System;
using Metabase.Enumerations;
using Metabase.GraphQl.Numerations;

namespace Metabase.GraphQl.Standards;

public sealed record UpdateStandardInput(
    string? Title,
    string? Abstract,
    string? Section,
    int? Year,
    UpdateNumerationInput Numeration,
    Standardizer[] Standardizers,
    Uri? Locator
);