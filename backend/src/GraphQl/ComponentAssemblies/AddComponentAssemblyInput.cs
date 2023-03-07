using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public record AddComponentAssemblyInput(
          Guid AssembledComponentId,
          Guid PartComponentId,
          byte? Index,
          PrimeSurface? PrimeSurface
        );
}