using CSharpFunctionalExtensions;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class PersonMethodDeveloperRemoved
      : MethodDeveloperRemoved
    {
        public static PersonMethodDeveloperRemoved From(
            Guid methodDeveloperId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>> command
            )
        {
            return new PersonMethodDeveloperRemoved(
                methodDeveloperId: methodDeveloperId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PersonMethodDeveloperRemoved() { }
#nullable enable

        public PersonMethodDeveloperRemoved(
            Guid methodDeveloperId,
            Guid creatorId
            )
          : base(
              methodDeveloperId: methodDeveloperId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}