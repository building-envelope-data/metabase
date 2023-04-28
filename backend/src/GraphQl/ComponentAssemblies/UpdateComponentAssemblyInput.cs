using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed record UpdateComponentAssemblyInput(
          Guid AssembledComponentId,
          Guid PartComponentId,
          byte? Index,
          PrimeSurface? PrimeSurface
        );
}