using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentManufacturerMarketingInformation
      : Model
    {
        public Guid ComponentManufacturerId { get; }
        public Guid ComponentInformationId { get; }
        public Guid InstitutionInformationId { get; }

        public ComponentManufacturerMarketingInformation(
            Guid id,
            Guid componentManufacturerId,
            Guid componentInformationId,
            Guid institutionInformationId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            ComponentManufacturerId = componentManufacturerId;
            ComponentInformationId = componentInformationId;
            InstitutionInformationId = institutionInformationId;
        }
    }
}