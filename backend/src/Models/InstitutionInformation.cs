using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class InstitutionInformation
      : Validatable
    {
        public string Name { get; }
        public string? Abbreviation { get; }
        public string? Description { get; }
        public Uri? WebsiteLocator { get; }

        public InstitutionInformation(
            string name,
            string? abbreviation,
            string? description,
            Uri? websiteLocator
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
          EnsureValid();
        }

        public override bool IsValid()
        {
            return !(Name is null);
        }
    }
}