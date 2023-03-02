using System;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public record AddComponentAssemblyInput(
          Guid AssembledComponentId,
          Guid PartComponentId
        );
}