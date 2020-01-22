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

        public string Name { get; }
        public string? Abbreviation { get; }
        public string Description { get; }
        public DateTime? AvailableFrom { get; }
        public DateTime? AvailableUntil { get; }
        public IEnumerable<ValueObjects.ComponentCategory> Categories { get; }

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