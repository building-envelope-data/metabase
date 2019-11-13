using Guid = System.Guid;
using System.Collections.Generic;

namespace Icon.Domain
{
    public sealed class ComponentVersionView
    {
        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public ICollection<ComponentVersionManufacturerView> Manufacturers { get; set; }

        public ComponentVersionView()
        {
            Manufacturers = new List<ComponentVersionManufacturerView>();
        }
    }
}