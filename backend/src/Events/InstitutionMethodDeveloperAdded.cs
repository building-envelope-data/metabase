using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class InstitutionMethodDeveloperAdded
      : MethodDeveloperAdded
    {
        public static InstitutionMethodDeveloperAdded From(
            Guid methodDeveloperId,
            Commands.AddMethodDeveloper command
            )
        {
            return new InstitutionMethodDeveloperAdded(
                methodDeveloperId: methodDeveloperId,
                methodId: command.Input.MethodId,
                stakeholderId: command.Input.StakeholderId,
                creatorId: command.CreatorId
                );
        }

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
        }
    }
}