using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class Standard
      : Model
    {
        public ValueObjects.Title Title { get; }
        public ValueObjects.Abstract Abstract { get; }
        public ValueObjects.Section Section { get; }
        public ValueObjects.Year Year { get; }
        public ValueObjects.Numeration Numeration { get; }
        public IReadOnlyCollection<ValueObjects.Standardizer> Standardizers { get; }
        public ValueObjects.AbsoluteUri? Locator { get; }

        private Standard(
            ValueObjects.Id id,
            ValueObjects.Title title,
            ValueObjects.Abstract @abstract,
            ValueObjects.Section section,
            ValueObjects.Year year,
            ValueObjects.Numeration numeration,
            IReadOnlyCollection<ValueObjects.Standardizer> standardizers,
            ValueObjects.AbsoluteUri? locator,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Title = title;
            Abstract = @abstract;
            Section = section;
            Year = year;
            Numeration = numeration;
            Standardizers = standardizers;
            Locator = locator;
        }

        public static Result<Standard, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Title title,
            ValueObjects.Abstract @abstract,
            ValueObjects.Section section,
            ValueObjects.Year year,
            ValueObjects.Numeration numeration,
            IReadOnlyCollection<ValueObjects.Standardizer> standardizers,
            ValueObjects.AbsoluteUri? locator,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<Standard, Errors>(
                  new Standard(
            id: id,
            title: title,
            @abstract: @abstract,
            section: section,
            year: year,
            numeration: numeration,
            standardizers: standardizers,
            locator: locator,
            timestamp: timestamp
            )
                  );
        }
    }
}