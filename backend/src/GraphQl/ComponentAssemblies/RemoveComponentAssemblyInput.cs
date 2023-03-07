using System;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public record RemoveComponentAssemblyInput(
          Guid AssembledComponentId,
          Guid PartComponentId
        );
}