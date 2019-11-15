using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class InstitutionInformation
      : Model
    {
        public string Name { get; }
        public string Abbreviation { get; }
        public string? Description { get; }
        public Uri? WebsiteLocator { get; }

        public InstitutionInformation(
            Guid id,
            string name,
            string abbreviation,
            string? description,
            Uri? websiteLocator,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
        }
    }
}