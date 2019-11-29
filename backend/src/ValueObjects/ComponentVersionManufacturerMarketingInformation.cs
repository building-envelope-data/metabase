using System.Collections.Generic;
using Errors = Icon.Errors;
using Array = System.Array;
using Validatable = Icon.Validatable;
using Guid = System.Guid;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;

namespace Icon.ValueObjects
{
    public sealed class ComponentVersionManufacturerMarketingInformation
      : ValueObject
    {
        public ComponentInformation? ComponentVersionInformation { get; }
        public InstitutionInformation? InstitutionInformation { get; }

        private ComponentVersionManufacturerMarketingInformation(
            ComponentInformation? componentVersionInformation,
            InstitutionInformation? institutionInformation
            )
        {
            ComponentVersionInformation = componentVersionInformation;
            InstitutionInformation = institutionInformation;
        }

        public static Result<ComponentVersionManufacturerMarketingInformation, Errors> From(
            ComponentInformation? componentVersionInformation,
            InstitutionInformation? institutionInformation,
            IReadOnlyList<object>? path = null
            )
        {
            if (componentVersionInformation is null &&
                institutionInformation is null)
            {
                return Result.Failure<ComponentVersionManufacturerMarketingInformation, Errors>(
                    Errors.One(
                      message: "Both component version and institution information are unspecified",
                      code: ErrorCodes.InvalidValue,
                      path: path
                      )
                    );
            }

            return Result.Ok<ComponentVersionManufacturerMarketingInformation, Errors>(
                new ComponentVersionManufacturerMarketingInformation(
                  componentVersionInformation: componentVersionInformation,
                  institutionInformation: institutionInformation
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ComponentVersionInformation;
            yield return InstitutionInformation;
        }
    }
}