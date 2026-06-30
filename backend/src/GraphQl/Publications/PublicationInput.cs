using System;
using HotChocolate;
using Metabase.Data;
using Metabase.GraphQl.Scalars;

namespace Metabase.GraphQl.Publications;

public sealed record PublicationInput(
    string? Title,
    string? Abstract,
    string? Section,
    string[]? Authors,
    [property: GraphQLType<DoiType>] string? Doi,
    [property: GraphQLType<ArXivType>] string? ArXiv,
    [property: GraphQLType<MyUriType>] string? Urn,
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