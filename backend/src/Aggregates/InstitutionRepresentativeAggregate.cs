// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using Marten.Schema;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using Icon.Events;

namespace Icon.Aggregates
{
    public sealed class InstitutionRepresentativeAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.InstitutionRepresentative>
    {
        [ForeignKey(typeof(InstitutionAggregate))]
        public Guid InstitutionId { get; set; }

        /* TODO [ForeignKey(typeof(UserAggregate))] */
        public Guid UserId { get; set; }

        public ValueObjects.InstitutionRepresentativeRole Role { get; set; }

        public Guid ParentId { get => InstitutionId; }
        public Guid AssociateId { get => UserId; }

#nullable disable
        public InstitutionRepresentativeAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.InstitutionRepresentativeAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            InstitutionId = data.InstitutionId;
            UserId = data.UserId;
            Role = data.Role.ToModel();
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                // All default values are listed under
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(InstitutionId, nameof(InstitutionId)),
                    ValidateEmpty(UserId, nameof(UserId)),
                    ValidateEquality(Role, ValueObjects.InstitutionRepresentativeRole.OWNER, nameof(Role))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  ValidateNonEmpty(UserId, nameof(UserId)),
                  ValidateNonNull(Role, nameof(Role))
                  );
        }

        public Result<Models.InstitutionRepresentative, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.InstitutionRepresentative, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var institutionIdResult = ValueObjects.Id.From(InstitutionId);
            var userIdResult = ValueObjects.Id.From(UserId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  institutionIdResult,
                  userIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.InstitutionRepresentative.From(
                    id: idResult.Value,
                    institutionId: institutionIdResult.Value,
                    userId: userIdResult.Value,
                    role: Role,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}