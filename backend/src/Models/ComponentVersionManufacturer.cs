using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class ComponentVersionManufacturer
      : Model
    {
        public Guid ComponentVersionId { get; }
        public Guid InstitutionId { get; }
        public ComponentVersionManufacturerMarketingInformation? MarketingInformation { get; }

        public ComponentVersionManufacturer(
            Guid id,
            Guid componentVersionId,
            Guid institutionId,
            ComponentVersionManufacturerMarketingInformation? marketingInformation,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            ComponentVersionId = componentVersionId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }
    }
}