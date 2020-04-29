using System.Collections.Generic;
using System.Linq;
using Errors = Icon.Errors;
using Array = System.Array;
using Validatable = Icon.Validatable;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;

namespace Icon.GraphQl
{
    public sealed class ComponentManufacturerMarketingInformationInput
    {
        public ComponentInformationInput? ComponentInformation { get; }
        public InstitutionInformationInput? InstitutionInformation { get; }

        private ComponentManufacturerMarketingInformationInput(
            ComponentInformationInput? componentInformation,
            InstitutionInformationInput? institutionInformation
            )
        {
            ComponentInformation = componentInformation;
            InstitutionInformation = institutionInformation;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.ComponentManufacturerMarketingInformation, Errors> Validate(
            ComponentManufacturerMarketingInformationInput self,
            IReadOnlyList<object> path
            )
        {
            // Why typing `null` is necessary here is explained in
            // https://stackoverflow.com/questions/18260528/type-of-conditional-expression-cannot-be-determined-because-there-is-no-implicit/18260915#18260915
            var componentInformationResult =
              self.ComponentInformation is null
              ? null as Result<ValueObjects.ComponentInformation, Errors>?
              : ComponentInformationInput.Validate(
                self.ComponentInformation,
                path.Append("componentInformation").ToList().AsReadOnly()
                );
            // Why typing `null` is necessary here is explained in
            // https://stackoverflow.com/questions/18260528/type-of-conditional-expression-cannot-be-determined-because-there-is-no-implicit/18260915#18260915
            var institutionInformationResult =
              self.InstitutionInformation is null
              ? null as Result<ValueObjects.InstitutionInformation, Errors>?
              : InstitutionInformationInput.Validate(
                self.InstitutionInformation,
                path.Append("institutionInformation").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  componentInformationResult,
                  institutionInformationResult
                  )
              .Bind(_ =>
                  ValueObjects.ComponentManufacturerMarketingInformation.From(
                    componentInformation: componentInformationResult?.Value,
                    institutionInformation: institutionInformationResult?.Value
                    )
                  );
        }
    }
}