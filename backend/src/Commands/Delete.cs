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
    public sealed class Delete<TModel>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.TimestampedId TimestampedId { get; }

        private Delete(
            ValueObjects.TimestampedId timestampedId,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            TimestampedId = timestampedId;
        }

        public static Result<Delete<TModel>, Errors> From(
            ValueObjects.TimestampedId timestampedId,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<Delete<TModel>, Errors>(
                    new Delete<TModel>(
                        timestampedId: timestampedId,
                        creatorId: creatorId
                        )
                    );
        }
    }
}