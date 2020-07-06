using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class InstitutionMethodDeveloperAdded
      : MethodDeveloperAdded
    {
        public static InstitutionMethodDeveloperAdded From(
            Guid methodDeveloperId,
            Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput> command
            )
        {
            return new InstitutionMethodDeveloperAdded(
                methodDeveloperId: methodDeveloperId,
                methodId: command.Input.MethodId,
                stakeholderId: command.Input.StakeholderId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public InstitutionMethodDeveloperAdded() { }
#nullable enable

        public InstitutionMethodDeveloperAdded(
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