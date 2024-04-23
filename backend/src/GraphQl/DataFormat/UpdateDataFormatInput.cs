using System;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.DataFormats;

public sealed record UpdateDataFormatInput(
    Guid DataFormatId,
    string Name,
    string? Extension,
    string Description,
    string MediaType,
    Uri? SchemaLocator,
    UpdateStandardInput? Standard,
    UpdatePublicationInput? Publication
);