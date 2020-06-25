using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddMethodDeveloperInput
      : AddOrRemoveMethodDeveloperInput
    {
        public AddMethodDeveloperInput(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId
            )
          : base(
              methodId: methodId,
              stakeholderId: stakeholderId
              )
        {
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