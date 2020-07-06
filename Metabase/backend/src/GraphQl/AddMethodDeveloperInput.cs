using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddMethodDeveloperInput
      : AddOrRemoveMethodDeveloperInput
    {
        public AddMethodDeveloperInput(
            Id methodId,
            Id stakeholderId
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