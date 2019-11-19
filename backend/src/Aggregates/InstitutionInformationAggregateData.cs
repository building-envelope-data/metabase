using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Aggregates
{
    public sealed class InstitutionInformationAggregateData
    {
        public static InstitutionInformationAggregateData From(
            Events.InstitutionInformationEventData information
            )
        {
            return new InstitutionInformationAggregateData(
                name: information.Name,
                abbreviation: information.Abbreviation,
                description: information.Description,
                websiteLocator: information.WebsiteLocator
                );
        }

        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public Uri WebsiteLocator { get; set; }

        public InstitutionInformationAggregateData() { }

        public InstitutionInformationAggregateData(
            string name,
            string abbreviation,
            string description,
            Uri websiteLocator
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
        }

        public Models.InstitutionInformation ToModel()
        {
            return new Models.InstitutionInformation(
                name: Name,
                abbreviation: Abbreviation,
                description: Description,
                websiteLocator: WebsiteLocator
                );
        }
    }
}