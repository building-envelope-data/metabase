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
    public sealed class Remove<TInput>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private Remove(
            TInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<Remove<TInput>, Errors> From(
            TInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<Remove<TInput>, Errors>(
                    new Remove<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}