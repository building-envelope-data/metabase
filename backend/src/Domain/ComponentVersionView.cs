using Guid = System.Guid;
using System.Collections.Generic;

namespace Icon.Domain
{
    public sealed class ComponentVersionView
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public List<ComponentVersionOwnershipView> Ownerships { get; set; }
    }
}