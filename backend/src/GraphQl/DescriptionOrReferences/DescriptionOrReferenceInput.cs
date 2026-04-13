using System;
using Metabase.Data;
using Metabase.GraphQl.References;

namespace Metabase.GraphQl.DescriptionOrReferences;

public sealed record DescriptionOrReferenceInput(
    ReferenceInput? Reference,
    string? Description
)
{
    public DescriptionOrReference? ToDomainModel()
    {
        if (Description is null && Reference?.Standard is null && Reference?.Publication is null)
        {
            return null;
        }
        var reference = Reference?.ToDomainModel();
        if (reference is null && Description is not null)
        {
            return new DescriptionOrReference(Description);
        }
        if (reference is not null && Description is null)
        {
            return new DescriptionOrReference(reference);
        }
        if (reference is not null && Description is not null)
        {
            return new DescriptionOrReference(reference, Description);
        }
        throw new InvalidOperationException("Impossible!");
    }
};