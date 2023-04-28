using System;

namespace Metabase.GraphQl.Components
{
    public sealed record UpdateComponentInput(
          Guid ComponentId,
          string Name,
          string? Abbreviation,
          string Description,
          Common.OpenEndedDateTimeRangeInput? Availability, // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_
          Enumerations.ComponentCategory[] Categories
        );
}