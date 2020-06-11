using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using CancellationToken = System.Threading.CancellationToken;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Commands
{
    public sealed class AddAssociation<TInput>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private AddAssociation(
            TInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddAssociation<TInput>, Errors> From(
            TInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<AddAssociation<TInput>, Errors>(
                    new AddAssociation<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}