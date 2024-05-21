using System;
using Metabase.Enumerations;
using Metabase.GraphQl.Numerations;

namespace Metabase.GraphQl.Standards;

public sealed record CreateStandardInput(
    string? Title,
    string? Abstract,
    string? Section,
    int? Year,
    CreateNumerationInput Numeration,
    Standardizer[] Standardizers,
    Uri? Locator
);