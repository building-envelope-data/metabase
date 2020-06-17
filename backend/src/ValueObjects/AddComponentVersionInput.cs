using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentVersionInput
      : AddManyToManyAssociationInput
    {
        public Id BaseComponentId { get => ParentId; }
        public Id VersionComponentId { get => AssociateId; }

        private AddComponentVersionInput(
            Id baseComponentId,
            Id versionComponentId
            )
          : base(
              parentId: baseComponentId,
              associateId: versionComponentId
              )
        {
        }

        public static Result<AddComponentVersionInput, Errors> From(
            Id baseComponentId,
            Id versionComponentId
            )
        {
            return
              Result.Ok<AddComponentVersionInput, Errors>(
                  new AddComponentVersionInput(
                    baseComponentId: baseComponentId,
                    versionComponentId: versionComponentId
                    )
                  );
        }
    }
}