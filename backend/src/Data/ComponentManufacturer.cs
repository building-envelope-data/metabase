using System;

namespace Metabase.Data
{
    public sealed class ComponentManufacturer
    {
        public Guid ComponentId { get; set; }
        public Component Component { get; set; } = default!;

        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; } = default!;

        // TODO public Enumerations.ComponentManufacturerMarketingInformation? MarketingInformation { get; }
    }
}