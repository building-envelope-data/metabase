using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class OpticalDataIgsdb
    : DataIgsdb
{
    public OpticalDataIgsdb(
        string id,
        Guid? uuid,
        DateTime timestamp,
        Guid componentId,
        string? name,
        string? description,
        GetHttpsResourceTreeIgsdb resourceTree,
        IReadOnlyList<double> nearnormalHemisphericalVisibleTransmittances,
        IReadOnlyList<double> nearnormalHemisphericalVisibleReflectances,
        IReadOnlyList<double> nearnormalHemisphericalSolarTransmittances,
        IReadOnlyList<double> nearnormalHemisphericalSolarReflectances,
        IReadOnlyList<double> infraredEmittances
    ) : base(
        id,
        uuid,
        timestamp,
        componentId,
        name,
        description,
        resourceTree
    )
    {
        NearnormalHemisphericalVisibleTransmittances = nearnormalHemisphericalVisibleTransmittances;
        NearnormalHemisphericalVisibleReflectances = nearnormalHemisphericalVisibleReflectances;
        NearnormalHemisphericalSolarTransmittances = nearnormalHemisphericalSolarTransmittances;
        NearnormalHemisphericalSolarReflectances = nearnormalHemisphericalSolarReflectances;
        InfraredEmittances = infraredEmittances;
    }

    public IReadOnlyList<double> NearnormalHemisphericalVisibleTransmittances { get; }
    public IReadOnlyList<double> NearnormalHemisphericalVisibleReflectances { get; }
    public IReadOnlyList<double> NearnormalHemisphericalSolarTransmittances { get; }
    public IReadOnlyList<double> NearnormalHemisphericalSolarReflectances { get; }
    public IReadOnlyList<double> InfraredEmittances { get; }
}