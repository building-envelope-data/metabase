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
    public sealed class CalorimetricDataCreated
      : CreatedEvent
    {
        public static CalorimetricDataCreated From(
            Guid calorimetricDataId,
            Commands.Create<ValueObjects.CreateCalorimetricDataInput> command
            )
        {
            return new CalorimetricDataCreated(
                calorimetricDataId: calorimetricDataId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

        public object? Data { get; set; }

#nullable disable
        public CalorimetricDataCreated() { }
#nullable enable

        public CalorimetricDataCreated(
            Guid calorimetricDataId,
            object? data,
            Guid creatorId
            )
          : base(
              aggregateId: calorimetricDataId,
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