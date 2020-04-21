using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class PersonMethodDeveloperAdded
      : MethodDeveloperAdded
    {
        public static PersonMethodDeveloperAdded From(
            Guid methodDeveloperId,
            Commands.AddMethodDeveloper command
            )
        {
            return new PersonMethodDeveloperAdded(
                methodDeveloperId: methodDeveloperId,
                methodId: command.Input.MethodId,
                stakeholderId: command.Input.StakeholderId,
                creatorId: command.CreatorId
                );
        }

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
        }
    }
}