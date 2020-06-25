using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class InstitutionMethodDeveloperRemoved
      : MethodDeveloperRemoved
    {
        public static InstitutionMethodDeveloperRemoved From(
            Guid methodDeveloperId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>> command
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