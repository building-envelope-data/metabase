using System.Collections.Generic;
using Infrastructure.ValueObjects;
using Uri = System.Uri;

namespace Metabase.GraphQl
{
    public sealed class Standard
      : NodeBase
    {
        public static Standard FromModel(
            Models.Standard model,
            Timestamp requestTimestamp
            )
        {
            return new Standard(
                id: model.Id,
                title: model.Title,
                @abstract: model.Abstract,
                section: model.Section,
                year: model.Year,
                prefix: model.Numeration.Prefix?.Value,
                mainNumber: model.Numeration.MainNumber,
                suffix: model.Numeration.Suffix?.Value,
                standardizers: model.Standardizers,
                locator: model.Locator?.Value,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public string Title { get; }
        public string Abstract { get; }
        public string Section { get; }
        public int Year { get; }
        public string? Prefix { get; }
        public string MainNumber { get; }
        public string? Suffix { get; }
        public IEnumerable<ValueObjects.Standardizer> Standardizers { get; }
        public Uri? Locator { get; }

        public Standard(
            Id id,
            string title,
            string @abstract,
            string section,
            int year,
            string? prefix,
            string mainNumber,
            string? suffix,
            IEnumerable<ValueObjects.Standardizer> standardizers,
            Uri? locator,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
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
        }
    }
}