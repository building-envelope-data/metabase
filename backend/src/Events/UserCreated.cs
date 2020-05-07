using Icon;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using System.Collections.Generic;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class UserCreated
      : CreatedEvent
    {
        public static UserCreated From(
            Guid userId,
            Commands.Create<ValueObjects.CreateUserInput> command
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