using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class ComponentManufacturerMarketingInformation
      : ValueObject
    {
        public ComponentInformation? ComponentInformation { get; }
        public InstitutionInformation? InstitutionInformation { get; }

        private ComponentManufacturerMarketingInformation(
            ComponentInformation? componentInformation,
            InstitutionInformation? institutionInformation
            )
        {
            ComponentInformation = componentInformation;
            InstitutionInformation = institutionInformation;
        }

        public static Result<ComponentManufacturerMarketingInformation, Errors> From(
            ComponentInformation? componentInformation,
            InstitutionInformation? institutionInformation,
            IReadOnlyList<object>? path = null
            )
        {
            if (componentInformation is null &&
                institutionInformation is null)
            {
                return Result.Failure<ComponentManufacturerMarketingInformation, Errors>(
                    Errors.One(
                      message: "Both component version and institution information are unspecified",
                      code: ErrorCodes.InvalidValue,
                      path: path
                      )
                    );
            }

            return Result.Ok<ComponentManufacturerMarketingInformation, Errors>(
                new ComponentManufacturerMarketingInformation(
                  componentInformation: componentInformation,
                  institutionInformation: institutionInformation
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ComponentInformation;
            yield return InstitutionInformation;
        }
    }
}