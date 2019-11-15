using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersionManufacturerMarketingInformation
    {
        public Guid ComponentVersionManufacturerId { get; }
        public Guid ComponentVersionInformationId { get; }
        public Guid InstitutionInformationId { get; }

        public ComponentVersionManufacturerMarketingInformation(
            Guid componentVersionManufacturerId,
            Guid componentVersionInformationId,
            Guid institutionInformationId
            )
        {
            ComponentVersionManufacturerId = componentVersionManufacturerId;
            ComponentVersionInformationId = componentVersionInformationId;
            InstitutionInformationId = institutionInformationId;
        }
    }
}