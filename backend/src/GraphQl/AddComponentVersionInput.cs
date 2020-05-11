using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentVersionInput
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VersionComponentId { get; }

        private AddComponentVersionInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }

        public static Result<ValueObjects.AddComponentVersionInput, Errors> Validate(
            AddComponentVersionInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddComponentVersionInput.From(
                    baseComponentId: self.BaseComponentId,
                    versionComponentId: self.VersionComponentId
                  );
        }
    }
}