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
    // The difference between the words `remove` and `delete` is explained on
    // https://english.stackexchange.com/questions/52508/difference-between-delete-and-remove
    public sealed class RemoveAssociation<TInput>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private RemoveAssociation(
            TInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<RemoveAssociation<TInput>, Errors> From(
            TInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<RemoveAssociation<TInput>, Errors>(
                    new RemoveAssociation<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}