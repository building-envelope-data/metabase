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
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public InstitutionMethodDeveloperRemoved() { }
#nullable enable

        public InstitutionMethodDeveloperRemoved(
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