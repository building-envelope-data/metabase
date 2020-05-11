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