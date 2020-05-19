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