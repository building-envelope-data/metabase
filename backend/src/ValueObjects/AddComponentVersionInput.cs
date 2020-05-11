using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.ValueObjects
{
    public sealed class AddComponentVersionInput
      : ValueObject
    {
        public Id BaseComponentId { get; }
        public Id VersionComponentId { get; }

        private AddComponentVersionInput(
            Id baseComponentId,
            Id versionComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }

        public static Result<AddComponentVersionInput, Errors> From(
            Id baseComponentId,
            Id versionComponentId
            )
        {
            return
              Result.Ok<AddComponentVersionInput, Errors>(
                  new AddComponentVersionInput(
                    baseComponentId: baseComponentId,
                    versionComponentId: versionComponentId
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return BaseComponentId;
            yield return VersionComponentId;
        }
    }
}