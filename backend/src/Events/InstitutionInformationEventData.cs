using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Events
{
    public sealed class InstitutionInformationEventData
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public Uri WebsiteLocator { get; set; }

        public InstitutionInformationEventData() { }

        public InstitutionInformationEventData(Models.InstitutionInformation information)
        {
          Name = information.Name;
          Abbreviation = information.Abbreviation;
          Description = information.Description;
          WebsiteLocator = information.WebsiteLocator;
        }
    }
}
