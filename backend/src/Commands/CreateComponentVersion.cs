using System.Collections.Generic;
using DateTime = System.DateTime;
using ValueObjects = Icon.ValueObjects;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class CreateComponentVersion
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.ComponentVersionInput Input { get; }

        private CreateComponentVersion(
            ValueObjects.ComponentVersionInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateComponentVersion, Errors> From(
            ValueObjects.ComponentVersionInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<CreateComponentVersion, Errors>(
                    new CreateComponentVersion(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}