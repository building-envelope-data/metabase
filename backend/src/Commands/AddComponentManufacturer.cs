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
    public sealed class AddComponentManufacturer
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public ValueObjects.AddComponentManufacturerInput Input { get; }

        private AddComponentManufacturer(
            ValueObjects.AddComponentManufacturerInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddComponentManufacturer, Errors> From(
            ValueObjects.AddComponentManufacturerInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<AddComponentManufacturer, Errors>(
                    new AddComponentManufacturer(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}