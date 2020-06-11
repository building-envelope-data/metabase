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
    public sealed class HygrothermalDataCreated
      : CreatedEvent
    {
        public static HygrothermalDataCreated From(
            Guid hygrothermalDataId,
            Commands.Create<ValueObjects.CreateHygrothermalDataInput> command
            )
        {
            return new HygrothermalDataCreated(
                hygrothermalDataId: hygrothermalDataId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

        public object? Data { get; set; }

#nullable disable
        public HygrothermalDataCreated() { }
#nullable enable

        public HygrothermalDataCreated(
            Guid hygrothermalDataId,
            object? data,
            Guid creatorId
            )
          : base(
              aggregateId: hygrothermalDataId,
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