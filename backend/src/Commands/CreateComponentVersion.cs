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
      : CommandBase<Result<(ValueObjects.Id, ValueObjects.Timestamp), IError>>
    {
        public ValueObjects.ComponentVersionInput Input { get; }

        private CreateComponentVersion(
            ValueObjects.ComponentVersionInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            ComponentId = componentId;
            Information = information;
        }

        public static Result<CreateComponentVersion, IError> From(
            ValueObjects.ComponentVersionInput input,
            ValueObjects.Id creatorId
            )
        {
					return Result.Ok(
							new CreateComponentVersion(
                input: input,
								creatorId: creatorId
								)
							);
    }
}
}