using Icon;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Uri = System.Uri;
using System.Collections.Generic;
using DateTime = System.DateTime;
using System.Threading.Tasks;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;
using System.Linq;

namespace Icon.Events
{
    public sealed class MethodCreated
      : CreatedEvent
    {
        public static MethodCreated From(
            Guid methodId,
            Commands.Create<ValueObjects.CreateMethodInput> command
            )
        {
            return new MethodCreated(
                methodId: methodId,
                name: command.Input.Name,
                description: command.Input.Description,
                standardId: command.Input.StandardId?.Value,
                publicationLocator: command.Input.PublicationLocator?.Value,
                codeLocator: command.Input.CodeLocator?.Value,
                categories: command.Input.Categories.Select(c => c.FromModel()).ToList().AsReadOnly(),
                creatorId: command.CreatorId
                );
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? StandardId { get; set; }
        public Uri? PublicationLocator { get; set; }
        public Uri? CodeLocator { get; set; }
        public IReadOnlyCollection<MethodCategoryEventData> Categories { get; set; }

#nullable disable
        public MethodCreated() { }
#nullable enable

        public MethodCreated(
            Guid methodId,
            string name,
            string description,
            Guid? standardId,
            Uri? publicationLocator,
            Uri? codeLocator,
            IReadOnlyCollection<MethodCategoryEventData> categories,
            Guid creatorId
            )
          : base(
              aggregateId: methodId,
              creatorId: creatorId
              )
        {
            Name = name;
            Description = description;
            StandardId = standardId;
            PublicationLocator = publicationLocator;
            CodeLocator = codeLocator;
            Categories = categories;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(Description, nameof(Description)),
                  ValidateNullOrNonEmpty(StandardId, nameof(StandardId)),
                  ValidateNonNull(Categories, nameof(Categories))
                  );
        }
    }
}