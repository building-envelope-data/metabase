using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Uri = System.Uri;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class Standard
      : Model
    {
        public ValueObjects.Title Title { get; }
        public ValueObjects.Abstract Abstract { get; }
        public ValueObjects.Section Section { get; }
        public ValueObjects.Year Year { get; }
        public ValueObjects.Prefix? Prefix { get; }
        public ValueObjects.MainNumber MainNumber { get; }
        public ValueObjects.Suffix? Suffix { get; }
        public IReadOnlyCollection<ValueObjects.Standardizer> Standardizers { get; }
        public ValueObjects.AbsoluteUri? Locator { get; }

        private Standard(
            ValueObjects.Id id,
            ValueObjects.Title title,
            ValueObjects.Abstract @abstract,
            ValueObjects.Section section,
            ValueObjects.Year year,
            ValueObjects.Prefix? prefix,
            ValueObjects.MainNumber mainNumber,
            ValueObjects.Suffix? suffix,
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
            Prefix = prefix;
            MainNumber = mainNumber;
            Suffix = suffix;
            Standardizers = standardizers;
            Locator = locator;
        }

        public static Result<Standard, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Title title,
            ValueObjects.Abstract @abstract,
            ValueObjects.Section section,
            ValueObjects.Year year,
            ValueObjects.Prefix? prefix,
            ValueObjects.MainNumber mainNumber,
            ValueObjects.Suffix? suffix,
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
            prefix: prefix,
            mainNumber: mainNumber,
            suffix: suffix,
            standardizers: standardizers,
            locator: locator,
            timestamp: timestamp
            )
                  );
        }
    }
}