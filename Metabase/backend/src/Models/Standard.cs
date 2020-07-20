using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
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
            Id id,
            ValueObjects.Title title,
            ValueObjects.Abstract @abstract,
            ValueObjects.Section section,
            ValueObjects.Year year,
            ValueObjects.Numeration numeration,
            IReadOnlyCollection<ValueObjects.Standardizer> standardizers,
            ValueObjects.AbsoluteUri? locator,
            Timestamp timestamp
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
            Id id,
            ValueObjects.Title title,
            ValueObjects.Abstract @abstract,
            ValueObjects.Section section,
            ValueObjects.Year year,
            ValueObjects.Numeration numeration,
            IReadOnlyCollection<ValueObjects.Standardizer> standardizers,
            ValueObjects.AbsoluteUri? locator,
            Timestamp timestamp
            )
        {
            return
              Result.Success<Standard, Errors>(
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