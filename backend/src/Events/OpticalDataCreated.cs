using Icon;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Uri = System.Uri;
using System.Collections.Generic;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;
using JToken = Newtonsoft.Json.Linq.JToken;

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
