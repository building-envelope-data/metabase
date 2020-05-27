using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentVersionInput
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VersionComponentId { get; }

        protected AddOrRemoveComponentVersionInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }
    }
}