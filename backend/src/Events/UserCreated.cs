using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Events;
using Icon.Infrastructure;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Commands = Icon.Commands;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;

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