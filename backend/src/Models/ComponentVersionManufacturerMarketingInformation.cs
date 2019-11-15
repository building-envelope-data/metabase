using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class ComponentVersionManufacturerMarketingInformation
    {
        public ComponentInformation ComponentVersionInformation { get; }
        public InstitutionInformation InstitutionInformation { get; }

        public ComponentVersionManufacturerMarketingInformation(
            ComponentInformation componentVersionInformation,
            InstitutionInformation institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
        }
    }
}