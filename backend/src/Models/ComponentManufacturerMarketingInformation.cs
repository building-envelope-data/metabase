using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentManufacturerMarketingInformation
    {
        public Guid ComponentManufacturerId { get; }
        public Guid ComponentInformationId { get; }
        public Guid InstitutionInformationId { get; }

        public ComponentManufacturerMarketingInformation(
            Guid componentManufacturerId,
            Guid componentInformationId,
            Guid institutionInformationId
            )
        {
            ComponentManufacturerId = componentManufacturerId;
            ComponentInformationId = componentInformationId;
            InstitutionInformationId = institutionInformationId;
        }
    }
}