using System;

namespace Metabase.Data
{
    public sealed class ComponentVariant
    {
        public Guid OfComponentId { get; set; }
        public Component OfComponent { get; set; } = default!;

        public Guid ToComponentId { get; set; }
        public Component ToComponent { get; set; } = default!;
    }
}