using System;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentVersion>
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid BaseComponentId { get; set; }

        [ForeignKey(typeof(ComponentAggregate))]
        public Guid VersionComponentId { get; set; }

        public Guid ParentId { get => BaseComponentId; }
        public Guid AssociateId { get => VersionComponentId; }

#nullable disable
        public ComponentVersionAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentVersionAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            BaseComponentId = data.BaseComponentId;
            VersionComponentId = data.VersionComponentId;
        }

        private void Apply(Marten.Events.Event<Events.ComponentVersionRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(BaseComponentId, nameof(BaseComponentId)),
                    ValidateEmpty(VersionComponentId, nameof(VersionComponentId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(BaseComponentId, nameof(BaseComponentId)),
                  ValidateNonEmpty(VersionComponentId, nameof(VersionComponentId))
                  );
        }

        public Result<Models.ComponentVersion, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentVersion, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var baseComponentIdResult = ValueObjects.Id.From(BaseComponentId);
            var versionComponentIdResult = ValueObjects.Id.From(VersionComponentId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  baseComponentIdResult,
                  versionComponentIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentVersion.From(
                    id: idResult.Value,
                    baseComponentId: baseComponentIdResult.Value,
                    versionComponentId: versionComponentIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}