using System;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Marten.Schema;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class InstitutionOperatedDatabaseAggregate
      : Aggregate, IOneToManyAssociationAggregate, IConvertible<Models.InstitutionOperatedDatabase>
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

        public void Apply(Marten.Events.Event<Events.InstitutionOperatedDatabaseAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            InstitutionId = data.InstitutionId;
            DatabaseId = data.DatabaseId;
        }

        public void Apply(Marten.Events.Event<Events.InstitutionOperatedDatabaseRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
            {
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(InstitutionId, nameof(InstitutionId)),
                    ValidateEmpty(DatabaseId, nameof(DatabaseId))
                    );
            }
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

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var institutionIdResult = Infrastructure.ValueObjects.Id.From(InstitutionId);
            var databaseIdResult = Infrastructure.ValueObjects.Id.From(DatabaseId);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

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