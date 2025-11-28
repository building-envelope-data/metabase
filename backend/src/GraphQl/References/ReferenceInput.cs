using System;
using Metabase.Data;
using Metabase.GraphQl.Publications;
using Metabase.GraphQl.Standards;

namespace Metabase.GraphQl.References;

public sealed record ReferenceInput(
    StandardInput? Standard,
    PublicationInput? Publication
)
{
    public Reference? ToDomainModel()
    {
        if (Standard is null && Publication is null)
        {
            return null;
        }
        if (Standard is not null && Publication is not null)
        {
            throw new InvalidOperationException("Both the reference's standard and publication are non-null.");
        }
        if (Standard is not null)
        {
            return new Reference(Standard.ToDomainModel());
        }
        if (Publication is not null)
        {
            return new Reference(Publication.ToDomainModel());
        }
        throw new InvalidOperationException("Impossible!");
    }
};