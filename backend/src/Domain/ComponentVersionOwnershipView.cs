using DateInterval = NodaTime.DateInterval;
using Guid = System.Guid;

namespace Icon.Domain
{
    public sealed class ComponentVersionOwnershipView
    {
        public Guid Id { get; set; }
        public Guid ComponentVersionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public DateInterval Availability { get; set; }
    }
}