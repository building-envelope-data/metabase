using System.Collections.Generic;
using Metabase.GraphQl.Components;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.ComponentManufacturers;

public sealed class ConfirmComponentManufacturerPayload
{
    public InstitutionManufacturedComponentEdge? ManufacturedComponentEdge { get; }
    public ComponentManufacturerEdge? ComponentManufacturerEdge { get; }
    public IReadOnlyCollection<ConfirmComponentManufacturerError>? Errors { get; }

    public ConfirmComponentManufacturerPayload(
        Data.ComponentManufacturer componentManufacturer
    )
    {
        ManufacturedComponentEdge = new InstitutionManufacturedComponentEdge(componentManufacturer);
        ComponentManufacturerEdge = new ComponentManufacturerEdge(componentManufacturer);
    }

    public ConfirmComponentManufacturerPayload(
        IReadOnlyCollection<ConfirmComponentManufacturerError> errors
    )
    {
        Errors = errors;
    }

    public ConfirmComponentManufacturerPayload(
        ConfirmComponentManufacturerError error
    )
        : this(new[] { error })
    {
    }
}