using CSharpFunctionalExtensions;
using Guid = System.Guid;

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