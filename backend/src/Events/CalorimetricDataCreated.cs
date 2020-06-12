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
      : DataCreatedEvent
    {
        public static CalorimetricDataCreated From(
            Guid opticalDataId,
            Commands.Create<ValueObjects.CreateCalorimetricDataInput> command
            )
        {
            return new CalorimetricDataCreated(
                opticalDataId: opticalDataId,
                componentId: command.Input.ComponentId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public CalorimetricDataCreated() { }
#nullable enable

        public CalorimetricDataCreated(
            Guid opticalDataId,
            Guid componentId,
            object? data,
            Guid creatorId
            )
          : base(
              dataId: opticalDataId,
              componentId: componentId,
              data: data,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}