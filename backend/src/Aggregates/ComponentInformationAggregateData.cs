using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;
using System.Linq;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentInformationAggregateData
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public ICollection<Models.ComponentCategory> Categories { get; set; }

        public ComponentInformationAggregateData() { }

        public ComponentInformationAggregateData(Events.ComponentInformationEventData information)
        {
          Name = information.Name;
          Abbreviation = information.Abbreviation;
          Description = information.Description;
          AvailableFrom = information.AvailableFrom;
          AvailableUntil = information.AvailableUntil;
          Categories =
            information.Categories
            .Select(Events.ComponentCategoryEventDataExtensions.ToModel)
            .ToList();
        }

        public Models.ComponentInformation ToModel()
        {
              return new Models.ComponentInformation(
                    name: Name,
                    abbreviation: Abbreviation,
                    description: Description,
                    availableFrom: AvailableFrom,
                    availableUntil: AvailableUntil,
                    categories: Categories
                  );
        }
    }
}
