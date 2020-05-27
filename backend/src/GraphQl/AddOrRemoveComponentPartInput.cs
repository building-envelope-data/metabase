using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentPartInput
    {
        public ValueObjects.Id AssembledComponentId { get; }
        public ValueObjects.Id PartComponentId { get; }

        protected AddOrRemoveComponentPartInput(
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId
            )
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }
    }
}