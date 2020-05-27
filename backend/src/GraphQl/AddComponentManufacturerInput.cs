using Uri = System.Uri;
using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentManufacturerInput
      : AddOrRemoveComponentManufacturerInput
    {
        public ComponentManufacturerMarketingInformationInput? MarketingInformation { get; }

        public AddComponentManufacturerInput(
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ComponentManufacturerMarketingInformationInput? marketingInformation
            )
          : base(
              componentId: componentId,
              institutionId: institutionId
              )
        {
            MarketingInformation = marketingInformation;
        }

        public static Result<ValueObjects.AddComponentManufacturerInput, Errors> Validate(
            AddComponentManufacturerInput self,
            IReadOnlyList<object> path
            )
        {
            // Why typing `null` is necessary here is explained in
            // https://stackoverflow.com/questions/18260528/type-of-conditional-expression-cannot-be-determined-because-there-is-no-implicit/18260915#18260915
            var marketingInformationResult =
              self.MarketingInformation is null
              ? null as Result<ValueObjects.ComponentManufacturerMarketingInformation, Errors>?
              : ComponentManufacturerMarketingInformationInput.Validate(
                self.MarketingInformation,
                path.Append("marketingInformation").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  marketingInformationResult
                  )
              .Bind(_ =>
                  ValueObjects.AddComponentManufacturerInput.From(
                    componentId: self.ComponentId,
                    institutionId: self.InstitutionId,
                    marketingInformation: marketingInformationResult?.Value
                    )
                  );
        }
    }
}