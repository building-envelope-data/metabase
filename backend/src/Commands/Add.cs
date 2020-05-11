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
    public sealed class Add<TInput>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private Add(
            TInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<Add<TInput>, Errors> From(
            TInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<Add<TInput>, Errors>(
                    new Add<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}