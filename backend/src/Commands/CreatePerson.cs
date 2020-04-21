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
    public sealed class CreatePerson
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.CreatePersonInput Input { get; }

        private CreatePerson(
            ValueObjects.CreatePersonInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreatePerson, Errors> From(
            ValueObjects.CreatePersonInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreatePerson, Errors>(
                    new CreatePerson(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}