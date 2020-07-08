using System;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Marten.Schema;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public abstract class MethodDeveloperAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.MethodDeveloper>
    {
        [ForeignKey(typeof(MethodAggregate))]
        public Guid MethodId { get; set; }

        public abstract Guid StakeholderId { get; set; }

        public Guid ParentId { get => MethodId; }
        public Guid AssociateId { get => StakeholderId; }

#nullable disable
        public MethodDeveloperAggregate() { }
#nullable enable

        protected void ApplyData(Events.MethodDeveloperAdded data)
        {
            Id = data.AggregateId;
            MethodId = data.MethodId;
            StakeholderId = data.StakeholderId;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
            {
                return Result.Combine(
                    base.Validate(),
                    ValidateEmpty(MethodId, nameof(MethodId)),
                    ValidateEmpty(StakeholderId, nameof(StakeholderId))
                    );
            }
            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(MethodId, nameof(MethodId)),
                  ValidateNonEmpty(StakeholderId, nameof(StakeholderId))
                  );
        }

        public Result<Models.MethodDeveloper, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.MethodDeveloper, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var methodIdResult = Infrastructure.ValueObjects.Id.From(MethodId);
            var stakeholderIdResult = Infrastructure.ValueObjects.Id.From(StakeholderId);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  methodIdResult,
                  stakeholderIdResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.MethodDeveloper.From(
                    id: idResult.Value,
                    methodId: methodIdResult.Value,
                    stakeholderId: stakeholderIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}