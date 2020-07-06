using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Events;
using Guid = System.Guid;
using Uri = System.Uri;

namespace Icon.Events
{
    public sealed class StandardCreated
      : CreatedEvent
    {
        public static StandardCreated From(
            Guid standardId,
            Commands.Create<ValueObjects.CreateStandardInput> command
            )
        {
            return new StandardCreated(
                standardId: standardId,
                title: command.Input.Title,
                @abstract: command.Input.Abstract,
                section: command.Input.Section,
                year: command.Input.Year,
                prefix: command.Input.Numeration.Prefix?.Value,
                mainNumber: command.Input.Numeration.MainNumber,
                suffix: command.Input.Numeration.Suffix?.Value,
                standardizers: command.Input.Standardizers.Select(s => s.FromModel()).ToList().AsReadOnly(),
                locator: command.Input.Locator?.Value,
                creatorId: command.CreatorId
                );
        }

        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Section { get; set; }
        public int Year { get; set; }
        public string? Prefix { get; set; }
        public string MainNumber { get; set; }
        public string? Suffix { get; set; }
        public IReadOnlyCollection<StandardizerEventData> Standardizers { get; set; }
        public Uri? Locator { get; set; }

#nullable disable
        public StandardCreated() { }
#nullable enable

        public StandardCreated(
            Guid standardId,
            string title,
            string @abstract,
            string section,
            int year,
            string? prefix,
            string mainNumber,
            string? suffix,
            IReadOnlyCollection<StandardizerEventData> standardizers,
            Uri? locator,
            Guid creatorId
            )
          : base(
              aggregateId: standardId,
              creatorId: creatorId
              )
        {
            Title = title;
            Abstract = @abstract;
            Section = section;
            Year = year;
            Prefix = prefix;
            MainNumber = mainNumber;
            Suffix = suffix;
            Standardizers = standardizers;
            Locator = locator;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Title, nameof(Title)),
                  ValidateNonNull(Abstract, nameof(Abstract)),
                  ValidateNonNull(Section, nameof(Section)),
                  ValidateNonNull(Year, nameof(Year)),
                  ValidateNonNull(MainNumber, nameof(MainNumber)),
                  ValidateNonNull(Standardizers, nameof(Standardizers))
                  );
        }
    }
}