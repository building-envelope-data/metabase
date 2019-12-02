using System.Collections.Generic;
using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ComponentInformation
    {
        public static ComponentInformation FromModel(
            ValueObjects.ComponentInformation model
            )
        {
            return new ComponentInformation(
                name: model.Name,
                abbreviation: model.Abbreviation?.Value,
                description: model.Description,
                availableFrom: model.Availability?.Start,
                availableUntil: model.Availability?.End,
                categories: model.Categories
                );
        }

        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public IEnumerable<ValueObjects.ComponentCategory> Categories { get; set; }

#nullable disable
        public ComponentInformation() { }
#nullable enable

        public ComponentInformation(
            string name,
            string? abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            IEnumerable<ValueObjects.ComponentCategory> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
            Categories = categories;
        }
    }
}