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
    public abstract class DataCreatedEvent
      : CreatedEvent
    {
        public Guid ComponentId { get; set; }
        public object? Data { get; set; }

#nullable disable
        public DataCreatedEvent() { }
#nullable enable

        public DataCreatedEvent(
            Guid dataId,
            Guid componentId,
            object? data,
            Guid creatorId
            )
          : base(
              aggregateId: dataId,
              creatorId: creatorId
              )
        {
            Data = data;
            ComponentId = componentId;
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonNull(Data, nameof(Data))
                  );
        }
    }
}