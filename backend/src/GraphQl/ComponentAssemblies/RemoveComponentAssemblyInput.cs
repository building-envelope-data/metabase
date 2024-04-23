using System;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed record RemoveComponentAssemblyInput(
        Guid AssembledComponentId,
        Guid PartComponentId
    );
}