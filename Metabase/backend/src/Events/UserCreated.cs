using CSharpFunctionalExtensions;
using Infrastructure.Events;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class UserCreated
      : CreatedEvent
    {
        public static UserCreated From(
            Guid userId,
            Infrastructure.Commands.CreateCommand<ValueObjects.CreateUserInput> command
            )
        {
            return new UserCreated(
                userId: userId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public UserCreated() { }
#nullable enable

        public UserCreated(
            Guid userId,
            Guid creatorId
            )
          : base(
              aggregateId: userId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate()
                  );
        }
    }
}