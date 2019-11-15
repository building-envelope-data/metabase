using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Aggregates
{
    public sealed class InstitutionInformationAggregateData
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public Uri WebsiteLocator { get; set; }

        public InstitutionInformationAggregateData() { }

        public InstitutionInformationAggregateData(Events.InstitutionInformationEventData information)
        {
          Name = information.Name;
          Abbreviation = information.Abbreviation;
          Description = information.Description;
          WebsiteLocator = information.WebsiteLocator;
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
