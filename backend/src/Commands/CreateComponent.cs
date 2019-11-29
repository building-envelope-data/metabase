using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class CreateComponent
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.ComponentInput Input { get; }

        private CreateComponent(
            ValueObjects.ComponentInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateComponent, Errors> From(
            ValueObjects.ComponentInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreateComponent, Errors>(
                    new CreateComponent(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}