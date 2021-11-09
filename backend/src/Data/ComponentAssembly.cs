using System;

// TODO Make sure that association is transitive!
namespace Metabase.Data
{
    public sealed class ComponentAssembly
    {
        public Guid AssembledComponentId { get; set; }
        public Component AssembledComponent { get; set; } = default!;

        public Guid PartComponentId { get; set; }
        public Component PartComponent { get; set; } = default!;
    }
}