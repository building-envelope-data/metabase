using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class InstitutionMethodDeveloperRemoved
      : MethodDeveloperRemoved
    {
        public static InstitutionMethodDeveloperRemoved From(
            Guid methodDeveloperId,
            Infrastructure.Commands.RemoveAssociationCommand<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>> command
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