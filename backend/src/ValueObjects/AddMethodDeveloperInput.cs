using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class AddMethodDeveloperInput
      : AddManyToManyAssociationInput
    {
        public Id MethodId { get => ParentId; }
        public Id StakeholderId { get => AssociateId; }

        private AddMethodDeveloperInput(
            Id methodId,
            Id stakeholderId
            )
          : base(
              parentId: methodId,
              associateId: stakeholderId
              )
        {
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
    }
}