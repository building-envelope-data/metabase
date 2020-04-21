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
    public sealed class CreateStandard
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.CreateStandardInput Input { get; }

        private CreateStandard(
            ValueObjects.CreateStandardInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateStandard, Errors> From(
            ValueObjects.CreateStandardInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreateStandard, Errors>(
                    new CreateStandard(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}