using System;
using System.ComponentModel.DataAnnotations;

namespace Metabase.Data
{
    public sealed class Standard
      : Infrastructure.Data.Entity
    {
        [Required]
        [MinLength(1)]
        public string Title { get; private set; }

        [Required]
        [MinLength(1)]
        public string Abstract { get; private set; }

        [Required]
        [MinLength(1)]
        public string Section { get; private set; }

        [Required]
        [Range(0, 3000)]
        public int Year { get; private set; }

        [Required]
        public Numeration Numeration { get; set; } = default!;

        [Required]
        public Enumerations.Standardizer[] Standardizers { get; private set; }

        [Required]
        [Url]
        public Uri Locator { get; private set; }

#nullable disable
        public Standard()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public Standard(
            string title,
            string @abstract,
            string section,
            int year,
            Enumerations.Standardizer[] standardizers,
            Uri locator
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