using System.ComponentModel.DataAnnotations;

namespace Metabase.Data
{
    public sealed class Standard
      : Infrastructure.Data.Entity
    {
        [MinLength(1)]
        public string Title { get; private set; }

        [MinLength(1)]
        public string Abstract { get; private set; }

        [MinLength(1)]
        public string Section { get; private set; }

        [Range(0, 3000)]
        public int Year { get; private set; }

        public Numeration Numeration { get; set; }

        public Enumerations.Standardizer[] Standardizers { get; private set; }

        [Url]
        public string Locator { get; private set; }

        public Standard(
            string title,
            string @abstract,
            string section,
            int year,
            Enumerations.Standardizer[] standardizers,
            string locator
            )
        {
            Title = title;
            Abstract = @abstract;
            Section = section;
            Year = year;
            Standardizers = standardizers;
            Locator = locator;
        }
    }
}