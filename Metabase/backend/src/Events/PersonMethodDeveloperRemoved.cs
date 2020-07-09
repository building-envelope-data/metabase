using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class PersonMethodDeveloperRemoved
      : MethodDeveloperRemoved
    {
        public static PersonMethodDeveloperRemoved From(
            Guid methodDeveloperId,
            Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.MethodDeveloper>> command
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