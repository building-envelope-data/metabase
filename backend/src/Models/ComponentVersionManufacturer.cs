using Guid = System.Guid;
using DateTime = System.DateTime;

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
          EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              ComponentVersionId != Guid.Empty &&
              InstitutionId != Guid.Empty &&
              (MarketingInformation?.IsValid() ?? true);
        }
    }
}