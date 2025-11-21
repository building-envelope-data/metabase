using System.Collections.Generic;
using System.Linq;

namespace Metabase.GraphQl.DataX;

public sealed record DataPropositionInput(
    // UuidPropositionInput? Id,
    UuidPropositionInput? ComponentId,
    IReadOnlyList<DataPropositionInput>? And,
    DataPropositionInput? Not,
    IReadOnlyList<DataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources
)
{
    public CalorimetricDataPropositionInput ToCalorimetricInput() => new(
        ComponentId,
        And?.Select(a => a.ToCalorimetricInput()).ToList().AsReadOnly(),
        Not?.ToCalorimetricInput(),
        Or?.Select(o => o.ToCalorimetricInput()).ToList().AsReadOnly(),
        Resources,
        null,
        null
    );

    public GeometricDataPropositionInput ToGeometricInput() => new(
        ComponentId,
        And?.Select(a => a.ToGeometricInput()).ToList().AsReadOnly(),
        Not?.ToGeometricInput(),
        Or?.Select(o => o.ToGeometricInput()).ToList().AsReadOnly(),
        Resources,
        null
    );

    public HygrothermalDataPropositionInput ToHygrothermalInput() => new(
        ComponentId,
        And?.Select(a => a.ToHygrothermalInput()).ToList().AsReadOnly(),
        Not?.ToHygrothermalInput(),
        Or?.Select(o => o.ToHygrothermalInput()).ToList().AsReadOnly(),
        Resources
    );

    public OpticalDataPropositionInput ToOpticalInput() => new(
        ComponentId,
        And?.Select(a => a.ToOpticalInput()).ToList().AsReadOnly(),
        Not?.ToOpticalInput(),
        Or?.Select(o => o.ToOpticalInput()).ToList().AsReadOnly(),
        Resources,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null
    );

    public PhotovoltaicDataPropositionInput ToPhotovoltaiInput() => new(
        ComponentId,
        And?.Select(a => a.ToPhotovoltaiInput()).ToList().AsReadOnly(),
        Not?.ToPhotovoltaiInput(),
        Or?.Select(o => o.ToPhotovoltaiInput()).ToList().AsReadOnly(),
        Resources
    );
};