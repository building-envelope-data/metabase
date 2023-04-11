using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public record UpdateComponentAssemblyInput(
          Guid AssembledComponentId,
          Guid PartComponentId,
          byte? Index,
          PrimeSurface? PrimeSurface
        );
}