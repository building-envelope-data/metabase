using CSharpFunctionalExtensions;
using Infrastructure.Events;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Database.Events
{
    public abstract class DataCreatedEvent
      : CreatedEvent
    {
        public Guid ComponentId { get; set; }
        public object? Data { get; set; }

#nullable disable
        protected DataCreatedEvent() { }
#nullable enable

        protected DataCreatedEvent(
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