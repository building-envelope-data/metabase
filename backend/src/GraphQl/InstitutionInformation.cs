using System.Collections.Generic;
using Uri = System.Uri;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class InstitutionInformation
    {
        public static InstitutionInformation FromModel(
            ValueObjects.InstitutionInformation model
            )
        {
            return new InstitutionInformation(
                name: model.Name,
                abbreviation: model.Abbreviation?.Value,
                description: model.Description?.Value,
                websiteLocator: model.WebsiteLocator?.Value
                );
        }

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
        }
    }
}