using System;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;

namespace Icon.Aggregates
{
    public sealed class DatabaseAggregate
      : EventSourcedAggregate, IConvertible<Models.Database>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Locator { get; set; }

#nullable disable
        public DatabaseAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.DatabaseCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            Name = data.Name;
            Description = data.Description;
            Locator = data.Locator;
        }

        private void Apply(Marten.Events.Event<Events.DatabaseDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Name, nameof(Name)),
                    ValidateNull(Description, nameof(Description)),
                    ValidateNull(Locator, nameof(Locator))
                  );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(Description, nameof(Description)),
                  ValidateNonNull(Locator, nameof(Locator))
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
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}