// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Marten.Schema;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class PersonAffiliationAggregate
      : EventSourcedAggregate, IConvertible<Models.PersonAffiliation>
    {
        [ForeignKey(typeof(PersonAggregate))]
        public Guid PersonId { get; set; }

        [ForeignKey(typeof(InstitutionAggregate))]
        public Guid InstitutionId { get; set; }

#nullable disable
        public PersonAffiliationAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.PersonAffiliationAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            PersonId = data.PersonId;
            InstitutionId = data.InstitutionId;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(PersonId, nameof(PersonId)),
                    ValidateEmpty(InstitutionId, nameof(InstitutionId))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(PersonId, nameof(PersonId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId))
                  );
        }

        public Result<Models.PersonAffiliation, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.PersonAffiliation, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var personIdResult = ValueObjects.Id.From(PersonId);
            var institutionIdResult = ValueObjects.Id.From(InstitutionId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  personIdResult,
                  institutionIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.PersonAffiliation.From(
                    id: idResult.Value,
                    personId: personIdResult.Value,
                    institutionId: institutionIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}