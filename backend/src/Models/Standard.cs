using System.Collections.Generic;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class Standard
      : Model
    {
        public string Title { get; }
        public string Abstract { get; }
        public string Section { get; }
        public DateTime Year { get; }
        public string? Prefix { get; }
        public string MainNumber { get; }
        public string? Suffix { get; }
        public IReadOnlyCollection<Standardizer> Standardizers { get; }
        public Uri? Locator { get; }

        public Standard(
            Guid id,
            string title,
            string @abstract,
            string section,
            DateTime year,
            string? prefix,
            string mainNumber,
            string? suffix,
            IReadOnlyCollection<Standardizer> standardizers,
            Uri? locator,
            DateTime timestamp
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
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              !string.IsNullOrWhiteSpace(Title) &&
              !string.IsNullOrWhiteSpace(Abstract) &&
              !string.IsNullOrWhiteSpace(Section) &&
              Year != DateTime.MinValue &&
              !(Prefix is null ? true : string.IsNullOrWhiteSpace(Prefix)) &&
              !string.IsNullOrWhiteSpace(MainNumber) &&
              !(Suffix is null ? true : string.IsNullOrWhiteSpace(Suffix)) &&
              !(Standardizers is null) &&
              (Locator?.IsAbsoluteUri ?? true);
        }
    }
}