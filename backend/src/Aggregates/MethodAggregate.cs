// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using Icon.Events;
using System.Linq;
using Marten.Schema;

namespace Icon.Aggregates
{
    public sealed class MethodAggregate
      : EventSourcedAggregate, IConvertible<Models.Method>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey(typeof(StandardAggregate))]
        public Guid? StandardId { get; set; }

        public Uri? PublicationLocator { get; set; }
        public Uri? CodeLocator { get; set; }
        public ICollection<ValueObjects.MethodCategory> Categories { get; set; }

#nullable disable
        public MethodAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.MethodCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            Name = data.Name;
            Description = data.Description;
            StandardId = data.StandardId;
            PublicationLocator = data.PublicationLocator;
            CodeLocator = data.CodeLocator;
            Categories = data.Categories
              .Select(Events.MethodCategoryEventDataExtensions.ToModel)
              .ToList();
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Name, nameof(Name)),
                    ValidateNull(Description, nameof(Description)),
                    ValidateNull(StandardId, nameof(StandardId)),
                    ValidateNull(PublicationLocator, nameof(PublicationLocator)),
                    ValidateNull(CodeLocator, nameof(CodeLocator)),
                    ValidateNull(Categories, nameof(Categories))
                    );

            return Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(Description, nameof(Description)),
                  ValidateNullOrNonEmpty(StandardId, nameof(StandardId)),
                  ValidateNonNull(Categories, nameof(Categories))
                  );
        }

        public Result<Models.Method, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Method, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var methodInformationResult =
              ValueObjects.MethodInformation.From(
                name: Name,
                description: Description,
                standardId: StandardId,
                publicationLocator: PublicationLocator,
                codeLocator: CodeLocator,
                categories: Categories.ToList().AsReadOnly()
                );
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Method.From(
                    id: idResult.Value,
                    information: methodInformationResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}