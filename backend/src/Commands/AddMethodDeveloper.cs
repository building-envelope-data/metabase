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
    public sealed class AddMethodDeveloper
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.AddMethodDeveloperInput Input { get; }

        private AddMethodDeveloper(
            ValueObjects.AddMethodDeveloperInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddMethodDeveloper, Errors> From(
            ValueObjects.AddMethodDeveloperInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<AddMethodDeveloper, Errors>(
                    new AddMethodDeveloper(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}