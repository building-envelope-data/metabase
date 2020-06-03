using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using Uri = System.Uri;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentManufacturerInput
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public AddOrRemoveComponentManufacturerInput(
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId
            )
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
        }
    }
}