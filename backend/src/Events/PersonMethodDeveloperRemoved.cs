using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class PersonMethodDeveloperRemoved
      : MethodDeveloperRemoved
    {
        public static PersonMethodDeveloperRemoved From(
            Guid methodDeveloperId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>> command
            )
        {
            return new PersonMethodDeveloperRemoved(
                methodDeveloperId: methodDeveloperId,
                methodId: command.Input.ParentId,
                stakeholderId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PersonMethodDeveloperRemoved() { }
#nullable enable

        public PersonMethodDeveloperRemoved(
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