using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Events
{
    public sealed class InstitutionInformationEventData
      : Validatable
    {
        public static InstitutionInformationEventData From(
            Models.InstitutionInformation information
            )
        {
            return new InstitutionInformationEventData(
                name: information.Name,
                abbreviation: information.Abbreviation,
                description: information.Description,
                websiteLocator: information.WebsiteLocator
                );
        }

        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public Uri? WebsiteLocator { get; set; }

#nullable disable
        public InstitutionInformationEventData() { }
#nullable enable

        public InstitutionInformationEventData(
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