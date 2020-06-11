// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class InstitutionAggregate
      : EventSourcedAggregate, IConvertible<Models.Institution>
    {
        public InstitutionInformationAggregateData Information { get; set; }
        public string? PublicKey { get; set; }
        public ValueObjects.InstitutionState State { get; set; }

#nullable disable
        public InstitutionAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.InstitutionCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            Information = InstitutionInformationAggregateData.From(data.Information);
            PublicKey = data.PublicKey;
            State = data.State.ToModel();
        }

        private void Apply(Marten.Events.Event<Events.InstitutionDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Information, nameof(Information)),
                    ValidateNull(PublicKey, nameof(PublicKey)),
                    ValidateEquality(State, ValueObjects.InstitutionState.UNKNOWN, nameof(State))
                    );

            return Result.Combine(
                base.Validate(),
                ValidateNonNull(Information, nameof(Information))
                .Bind(_ => Information.Validate()),
                ValidateNonNull(State, nameof(State))
                );
        }

        public Result<Models.Institution, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Institution, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var informationResult = Information.ToValueObject();
            var publicKeyResult = ValueObjects.PublicKey.MaybeFrom(PublicKey);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  informationResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Institution.From(
                    id: idResult.Value,
                    information: informationResult.Value,
                    publicKey: publicKeyResult?.Value,
                    state: State,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}