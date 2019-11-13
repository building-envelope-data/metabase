using Guid = System.Guid;
using System.Collections.Generic;

namespace Icon.Domain
{
    public sealed class ComponentView
    {
        public Guid Id { get; set; }
        public ICollection<ComponentVersionView> Versions { get; set; }

        public ComponentView()
        {
            Versions = new List<ComponentVersionView>();
        }
    }
}