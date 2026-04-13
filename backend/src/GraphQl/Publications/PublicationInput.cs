using System;
using Metabase.Data;

namespace Metabase.GraphQl.Publications;

public sealed record PublicationInput(
    string? Title,
    string? Abstract,
    string? Section,
    string[]? Authors,
    string? Doi,
    string? ArXiv,
    string? Urn,
    Uri? WebAddress
)
{
    public Publication ToDomainModel()
    {
        return new(
            Title,
            Abstract,
            Section,
            Authors,
            Doi,
            ArXiv,
            Urn,
            WebAddress
        );
    }
};