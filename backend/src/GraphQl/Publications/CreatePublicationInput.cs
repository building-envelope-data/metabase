using System;

namespace Metabase.GraphQl.Publications;

public sealed record CreatePublicationInput(
    string? Title,
    string? Abstract,
    string? Section,
    string[]? Authors,
    string? Doi,
    string? ArXiv,
    string? Urn,
    Uri? WebAddress
);