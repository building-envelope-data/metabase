using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Metabase.Data
{
    public sealed class Component
      : Infrastructure.Data.Entity
    {
        // Entity Framework Core Read-Only Properties https://docs.microsoft.com/en-us/ef/core/modeling/constructors#read-only-properties
        // Data Annotations https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations
        // Built-In Validation Attributes https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation#built-in-attributes

        [Required]
        [MinLength(1)]
        public string Name { get; private set; }

        [MinLength(1)]
        public string? Abbreviation { get; private set; }

        [Required]
        [MinLength(1)]
        public string Description { get; private set; }

        public NpgsqlRange<DateTime>? Availability { get; private set; } // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_

        // https://www.npgsql.org/efcore/mapping/array.html
        [Required]
        public Enumerations.ComponentCategory[] Categories { get; private set; }

        public ICollection<ComponentConcretizationAndGeneralization> ConcretizationEdges { get; } = new List<ComponentConcretizationAndGeneralization>();
        public ICollection<Component> Concretizations { get; } = new List<Component>();

        public ICollection<ComponentConcretizationAndGeneralization> GeneralizationEdges { get; } = new List<ComponentConcretizationAndGeneralization>();
        public ICollection<Component> Generalizations { get; } = new List<Component>();

        public ICollection<ComponentManufacturer> ManufacturerEdges { get; } = new List<ComponentManufacturer>();
        public ICollection<Institution> Manufacturers { get; } = new List<Institution>();

#nullable disable
        public Component()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public Component(
            string name,
            string? abbreviation,
            string description,
            NpgsqlRange<DateTime>? availability,
            Enumerations.ComponentCategory[] categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            Availability = availability;
            Categories = categories;
        }

        public void Update(
            string name,
            string? abbreviation,
            string description,
            NpgsqlRange<DateTime>? availability,
            Enumerations.ComponentCategory[] categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            Availability = availability;
            Categories = categories;
        }
    }
}