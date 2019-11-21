using Validatable = Icon.Validatable;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersionManufacturerMarketingInformation
      : Validatable
    {
        public ComponentInformation? ComponentVersionInformation { get; }
        public InstitutionInformation? InstitutionInformation { get; }

        public ComponentVersionManufacturerMarketingInformation(
            ComponentInformation? componentVersionInformation,
            InstitutionInformation? institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
        }

        public override bool IsValid()
        {
            return
              (ComponentVersionInformation?.IsValid() ?? true) &&
              (InstitutionInformation?.IsValid() ?? true);
        }
    }
}