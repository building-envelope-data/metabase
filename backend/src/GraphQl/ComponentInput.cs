using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class ComponentInput
    {
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public IReadOnlyCollection<Models.ComponentCategory>? Categories { get; set; }

        public ComponentInput() { }
    }
}