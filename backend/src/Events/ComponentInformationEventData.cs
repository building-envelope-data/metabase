using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;
using System.Linq;

namespace Icon.Events
{
    public sealed class ComponentInformationEventData
      : Validatable
    {
        public static ComponentInformationEventData From(
            Models.ComponentInformation information
            )
        {
            return new ComponentInformationEventData(
                  name: information.Name,
                  abbreviation: information.Abbreviation,
                  description: information.Description,
                  availableFrom: information.AvailableFrom,
                  availableUntil: information.AvailableUntil,
                  categories: information.Categories.Select(c => c.FromModel()).ToList()
                );
        }

        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public IReadOnlyCollection<ComponentCategoryEventData>? Categories { get; set; }

        public ComponentInformationEventData() { }

        public ComponentInformationEventData(
            string name,
            string? abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            IReadOnlyCollection<ComponentCategoryEventData> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
            Categories = categories;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return !(Name is null) &&
              !(Description is null) &&
              !(Categories is null);
        }
    }
}