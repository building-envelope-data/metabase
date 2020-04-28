// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using System.Linq;

namespace Icon.Aggregates
{
    public sealed class StandardAggregate
      : EventSourcedAggregate, IConvertible<Models.Standard>
    {
        public Guid StandardId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Section { get; set; }
        public int Year { get; set; }
        public string? Prefix { get; set; }
        public string MainNumber { get; set; }
        public string? Suffix { get; set; }
        public ICollection<ValueObjects.Standardizer> Standardizers { get; set; }
        public Uri? Locator { get; set; }

#nullable disable
        public StandardAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.StandardCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            Title = data.Title;
            Abstract = data.Abstract;
            Section = data.Section;
            Year = data.Year;
            Prefix = data.Prefix;
            MainNumber = data.MainNumber;
            Suffix = data.Suffix;
            Standardizers = data.Standardizers
              .Select(Events.StandardizerEventDataExtensions.ToModel)
              .ToList();
            Locator = data.Locator;
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                      base.Validate(),
                      ValidateEmpty(StandardId, nameof(StandardId)),
                      ValidateNull(Title, nameof(Title)),
                      ValidateNull(Abstract, nameof(Abstract)),
                      ValidateNull(Section, nameof(Section)),
                      ValidateZero(Year, nameof(Year)),
                      ValidateNull(Prefix, nameof(Prefix)),
                      ValidateNull(MainNumber, nameof(MainNumber)),
                      ValidateNull(Suffix, nameof(Suffix)),
                      ValidateNonNull(Standardizers, nameof(Standardizers)),
                      ValidateNull(Locator, nameof(Locator))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(StandardId, nameof(StandardId)),
                  ValidateNonNull(Title, nameof(Title)),
                  ValidateNonNull(Abstract, nameof(Abstract)),
                  ValidateNonNull(Section, nameof(Section)),
                  ValidateNonNull(Year, nameof(Year)),
                  ValidateNonNull(MainNumber, nameof(MainNumber)),
                  ValidateNonNull(Standardizers, nameof(Standardizers))
                  );
        }

        public Result<Models.Standard, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Standard, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var titleResult = ValueObjects.Title.From(Title);
            var abstractResult = ValueObjects.Abstract.From(Abstract);
            var sectionResult = ValueObjects.Section.From(Section);
            var yearResult = ValueObjects.Year.From(Year);
            var numerationResult = ValueObjects.Numeration.From(
                prefix: Prefix,
                mainNumber: MainNumber,
                suffix: Suffix
                );
            var locatorResult = ValueObjects.AbsoluteUri.MaybeFrom(Locator);
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.CombineExistent(
                  idResult,
                  titleResult,
                  abstractResult,
                  sectionResult,
                  yearResult,
                  numerationResult,
                  locatorResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Standard.From(
                    id: idResult.Value,
                    title: titleResult.Value,
                    @abstract: abstractResult.Value,
                    section: sectionResult.Value,
                    year: yearResult.Value,
                    numeration: numerationResult.Value,
                    standardizers: Standardizers.ToList().AsReadOnly(),
                    locator: locatorResult?.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}