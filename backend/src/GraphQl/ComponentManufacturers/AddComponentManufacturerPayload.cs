using System.Collections.Generic;
using Metabase.GraphQl.Components;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.ComponentManufacturers;

public sealed class AddComponentManufacturerPayload
{
    public InstitutionManufacturedComponentEdge? ManufacturedComponentEdge { get; }
    public ComponentManufacturerEdge? ComponentManufacturerEdge { get; }
    public IReadOnlyCollection<AddComponentManufacturerError>? Errors { get; }

    public AddComponentManufacturerPayload(
        Data.ComponentManufacturer componentManufacturer
    )
    {
        ManufacturedComponentEdge = new InstitutionManufacturedComponentEdge(componentManufacturer);
        ComponentManufacturerEdge = new ComponentManufacturerEdge(componentManufacturer);
    }

    public AddComponentManufacturerPayload(
        IReadOnlyCollection<AddComponentManufacturerError> errors
    )
    {
        Errors = errors;
    }

    public AddComponentManufacturerPayload(
        AddComponentManufacturerError error
    )
        : this(new[] { error })
    {
    }
}