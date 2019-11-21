using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentManufacturerMarketingInformation
    {
        public Guid ComponentManufacturerId { get; }
        public ComponentInformation? ComponentInformation { get; }
        public InstitutionInformation? InstitutionInformation { get; }

        public ComponentManufacturerMarketingInformation(
            Guid componentManufacturerId,
            ComponentInformation? componentInformation,
            InstitutionInformation? institutionInformation
            )
        {
            ComponentManufacturerId = componentManufacturerId;
            ComponentInformation = componentInformation;
            InstitutionInformation = institutionInformation;
        }
    }
}