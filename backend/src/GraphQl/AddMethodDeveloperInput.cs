using System.Collections.Generic;
using System.Linq;
using Guid = System.Guid;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddMethodDeveloperInput
    {
        public Guid MethodId { get; }
        public Guid StakeholderId { get; }

        private AddMethodDeveloperInput(
            Guid methodId,
            Guid stakeholderId
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
            var methodIdResult = ValueObjects.Id.From(
                self.MethodId,
                path.Append("methodId").ToList().AsReadOnly()
                );
            var stakeholderIdResult = ValueObjects.Id.From(
                self.StakeholderId,
                path.Append("stakeholderId").ToList().AsReadOnly()
                );

            return
              Errors.Combine(
                  methodIdResult,
                  stakeholderIdResult
                  )
              .Bind(_ =>
                  ValueObjects.AddMethodDeveloperInput.From(
                    methodId: methodIdResult.Value,
                    stakeholderId: stakeholderIdResult.Value
                    )
                  );
        }
    }
}