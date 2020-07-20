using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
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
              Result.Success<AddComponentVersionInput, Errors>(
                  new AddComponentVersionInput(
                    baseComponentId: baseComponentId,
                    versionComponentId: versionComponentId
                    )
                  );
        }
    }
}