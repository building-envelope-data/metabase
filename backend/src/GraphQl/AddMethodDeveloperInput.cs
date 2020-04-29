using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddMethodDeveloperInput
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        private AddMethodDeveloperInput(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId
            )
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.AddMethodDeveloperInput, Errors> Validate(
            AddMethodDeveloperInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddMethodDeveloperInput.From(
                    methodId: self.MethodId,
                    stakeholderId: self.StakeholderId
                  );
        }
    }
}