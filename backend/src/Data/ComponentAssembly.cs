using System;
using Metabase.Enumerations;

namespace Metabase.Data
{
    public sealed class ComponentAssembly
    {
        public Guid AssembledComponentId { get; set; }
        public Component AssembledComponent { get; set; } = default!;

        public Guid PartComponentId { get; set; }
        public Component PartComponent { get; set; } = default!;

        public byte? Index { get; set; }
        public PrimeSurface? PrimeSurface { get; set; }
    }
}