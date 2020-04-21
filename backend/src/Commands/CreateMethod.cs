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
    public sealed class CreateMethod
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.CreateMethodInput Input { get; }

        private CreateMethod(
            ValueObjects.CreateMethodInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateMethod, Errors> From(
            ValueObjects.CreateMethodInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreateMethod, Errors>(
                    new CreateMethod(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}