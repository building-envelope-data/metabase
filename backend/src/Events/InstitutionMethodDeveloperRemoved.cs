using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class InstitutionMethodDeveloperRemoved
      : MethodDeveloperRemoved
    {
        public static InstitutionMethodDeveloperRemoved From(
            Guid methodDeveloperId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>> command
            )
        {
            return new InstitutionMethodDeveloperRemoved(
                methodDeveloperId: methodDeveloperId,
                methodId: command.Input.ParentId,
                stakeholderId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public InstitutionMethodDeveloperRemoved() { }
#nullable enable

        public InstitutionMethodDeveloperRemoved(
            Guid methodDeveloperId,
            Guid methodId,
            Guid stakeholderId,
            Guid creatorId
            )
          : base(
              methodDeveloperId: methodDeveloperId,
              methodId: methodId,
              stakeholderId: stakeholderId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}