using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class PersonMethodDeveloperAdded
      : MethodDeveloperAdded
    {
        public static PersonMethodDeveloperAdded From(
            Guid methodDeveloperId,
            Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput> command
            )
        {
            return new PersonMethodDeveloperAdded(
                methodDeveloperId: methodDeveloperId,
                methodId: command.Input.MethodId,
                stakeholderId: command.Input.StakeholderId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PersonMethodDeveloperAdded() { }
#nullable enable

        public PersonMethodDeveloperAdded(
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