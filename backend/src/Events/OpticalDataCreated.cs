using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Events;
using Icon.Infrastructure;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Commands = Icon.Commands;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;
using JToken = Newtonsoft.Json.Linq.JToken;
using Uri = System.Uri;

namespace Icon.Events
{
    public sealed class OpticalDataCreated
      : CreatedEvent
    {
        public static OpticalDataCreated From(
            Guid opticalDataId,
            Commands.Create<ValueObjects.CreateOpticalDataInput> command
            )
        {
            return new OpticalDataCreated(
                opticalDataId: opticalDataId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

        public object? Data { get; set; }

#nullable disable
        public OpticalDataCreated() { }
#nullable enable

        public OpticalDataCreated(
            Guid opticalDataId,
            object? data,
            Guid creatorId
            )
          : base(
              aggregateId: opticalDataId,
              creatorId: creatorId
              )
        {
            Data = data;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Data, nameof(Data))
                  );
        }
    }
}