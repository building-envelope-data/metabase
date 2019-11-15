using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersionManufacturerMarketingInformation
      : Model
    {
        public Guid ComponentVersionManufacturerId { get; }
        public Guid ComponentVersionInformationId { get; }
        public Guid InstitutionInformationId { get; }

        public ComponentVersionManufacturerMarketingInformation(
            Guid id,
            Guid componentVersionManufacturerId,
            Guid componentVersionInformationId,
            Guid institutionInformationId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            ComponentVersionManufacturerId = componentVersionManufacturerId;
            ComponentVersionInformationId = componentVersionInformationId;
            InstitutionInformationId = institutionInformationId;
        }
    }
}