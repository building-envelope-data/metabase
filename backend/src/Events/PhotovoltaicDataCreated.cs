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
    public sealed class PhotovoltaicDataCreated
      : DataCreatedEvent
    {
        public static PhotovoltaicDataCreated From(
            Guid photovoltaicDataId,
            Commands.Create<ValueObjects.CreatePhotovoltaicDataInput> command
            )
        {
            return new PhotovoltaicDataCreated(
                photovoltaicDataId: photovoltaicDataId,
                componentId: command.Input.ComponentId,
                data: command.Input.Data.ToNestedCollections(),
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PhotovoltaicDataCreated() { }
#nullable enable

        public PhotovoltaicDataCreated(
            Guid photovoltaicDataId,
            Guid componentId,
            object? data,
            Guid creatorId
            )
          : base(
              dataId: photovoltaicDataId,
              componentId: componentId,
              data: data,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}