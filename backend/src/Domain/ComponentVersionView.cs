using Guid = System.Guid;
using System.Collections.Generic;

namespace Icon.Domain
{
    public sealed class ComponentVersionView
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public ICollection<ComponentVersionOwnershipView> Ownerships { get; private set; }

        public ComponentVersionView()
        {
          Ownerships = new List<ComponentVersionOwnershipView>();
        }
    }
}