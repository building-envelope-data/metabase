// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class DatabaseAggregate
      : EventSourcedAggregate, IConvertible<Models.Database>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Locator { get; set; }
        public Guid InstitutionId { get; set; }

#nullable disable
        public DatabaseAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.DatabaseCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.DatabaseId;
            Name = data.Name;
            Description = data.Description;
            Locator = data.Locator;
            InstitutionId = data.InstitutionId;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Name, nameof(Name)),
                    ValidateNull(Description, nameof(Description)),
                    ValidateNull(Locator, nameof(Locator)),
                    ValidateEmpty(InstitutionId, nameof(InstitutionId))
                  );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(Description, nameof(Description)),
                  ValidateNonNull(Locator, nameof(Locator)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId))
                  );
        }

        public Result<Models.Database, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Database, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var nameResult = ValueObjects.Name.From(Name);
            var descriptionResult = ValueObjects.Description.From(Description);
            var locatorResult = ValueObjects.AbsoluteUri.From(Locator);
            var institutionIdResult = ValueObjects.Id.From(InstitutionId);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Database.From(
                    id: idResult.Value,
                    name: nameResult.Value,
                    description: descriptionResult.Value,
                    locator: locatorResult.Value,
                    institutionId: institutionIdResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}