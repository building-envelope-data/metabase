using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class AddMethodDeveloperInput
      : ValueObject
    {
        public Id MethodId { get; }
        public Id StakeholderId { get; }

        private AddMethodDeveloperInput(
            Id methodId,
            Id stakeholderId
            )
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }

        public static Result<AddMethodDeveloperInput, Errors> From(
            Id methodId,
            Id stakeholderId
            )
        {
            return
              Result.Ok<AddMethodDeveloperInput, Errors>(
                  new AddMethodDeveloperInput(
                    methodId: methodId,
                    stakeholderId: stakeholderId
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return MethodId;
            yield return StakeholderId;
        }
    }
}