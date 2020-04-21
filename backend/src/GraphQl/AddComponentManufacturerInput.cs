using Uri = System.Uri;
using Guid = System.Guid;
using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentManufacturerInput
    {
        public Guid ComponentId { get; }
        public Guid InstitutionId { get; }
        public ComponentManufacturerMarketingInformationInput? MarketingInformation { get; }

        public AddComponentManufacturerInput(
            Guid componentId,
            Guid institutionId,
            ComponentManufacturerMarketingInformationInput? marketingInformation
            )
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.AddComponentManufacturerInput, Errors> Validate(
            AddComponentManufacturerInput self,
            IReadOnlyList<object> path
            )
        {
            var componentIdResult = ValueObjects.Id.From(
                self.ComponentId,
                path.Append("componentId").ToList().AsReadOnly()
                );
            var institutionIdResult = ValueObjects.Id.From(
                self.InstitutionId,
                path.Append("institutionId").ToList().AsReadOnly()
                );
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
                  componentIdResult,
                  institutionIdResult,
                  marketingInformationResult
                  )
              .Bind(_ =>
                  ValueObjects.AddComponentManufacturerInput.From(
                    componentId: componentIdResult.Value,
                    institutionId: institutionIdResult.Value,
                    marketingInformation: marketingInformationResult?.Value
                    )
                  );
        }
    }
}