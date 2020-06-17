using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class InstitutionOperatedDatabaseAggregate
      : EventSourcedAggregate, IOneToManyAssociationAggregate, IConvertible<Models.InstitutionOperatedDatabase>
    {
        [ForeignKey(typeof(InstitutionAggregate))]
        public Guid InstitutionId { get; set; }

        [ForeignKey(typeof(DatabaseAggregate))]
        public Guid DatabaseId { get; set; }

        public Guid ParentId { get => InstitutionId; }
        public Guid AssociateId { get => DatabaseId; }

#nullable disable
        public InstitutionOperatedDatabaseAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.InstitutionOperatedDatabaseAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            InstitutionId = data.InstitutionId;
            DatabaseId = data.DatabaseId;
        }

        private void Apply(Marten.Events.Event<Events.InstitutionOperatedDatabaseRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(InstitutionId, nameof(InstitutionId)),
                    ValidateEmpty(DatabaseId, nameof(DatabaseId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  ValidateNonEmpty(DatabaseId, nameof(DatabaseId))
                  );
        }

        public Result<Models.InstitutionOperatedDatabase, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.InstitutionOperatedDatabase, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var institutionIdResult = ValueObjects.Id.From(InstitutionId);
            var databaseIdResult = ValueObjects.Id.From(DatabaseId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  institutionIdResult,
                  databaseIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.InstitutionOperatedDatabase.From(
                    id: idResult.Value,
                    institutionId: institutionIdResult.Value,
                    databaseId: databaseIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}