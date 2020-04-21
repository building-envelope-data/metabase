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
    public sealed class CreateDatabase
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.CreateDatabaseInput Input { get; }

        private CreateDatabase(
            ValueObjects.CreateDatabaseInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateDatabase, Errors> From(
            ValueObjects.CreateDatabaseInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreateDatabase, Errors>(
                    new CreateDatabase(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}